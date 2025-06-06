﻿using System.Text.Json;
using HeyBox.API;
using HeyBox.API.Gateway;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

public partial class HeyBoxSocketClient
{
    #region Gateway

    private async Task HandlePongAsync()
    {
        await _gatewayLogger.DebugAsync("Received Pong").ConfigureAwait(false);
        if (_heartbeatTimes.TryDequeue(out long time))
        {
            int latency = (int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - time);
            int before = Latency;
            Latency = latency;

            await TimedInvokeAsync(_latencyUpdatedEvent, nameof(LatencyUpdated), before, latency)
                .ConfigureAwait(false);
        }
    }

    internal async Task FetchRequiredDataAsync()
    {
        // Get current user
        try
        {
            if (TokenUtils.TryParseBotTokenUserId(Token, out uint selfUserId))
            {
                SocketSelfUser currentUser = SocketSelfUser.Create(this, State, selfUserId);
                Rest.CreateRestSelfUser(currentUser.Id);
                ApiClient.CurrentUserId = currentUser.Id;
                Rest.CurrentUser = new RestSelfUser(this, currentUser.Id);
                CurrentUser = currentUser;
            }
        }
        catch (Exception ex)
        {
            Connection.CriticalError(new Exception("Processing SelfUser failed", ex));
            return;
        }

        // Download room data
        try
        {
            List<Room> rooms = (await ApiClient.GetJoinedRoomsAsync().FlattenAsync().ConfigureAwait(false)).ToList();
            // StartupCacheFetchMode = BaseConfig.StartupCacheFetchMode;
            // if (StartupCacheFetchMode is StartupCacheFetchMode.Auto)
            // {
            //     if (rooms.Count >= LargeNumberOfRoomsThreshold)
            //         StartupCacheFetchMode = StartupCacheFetchMode.Lazy;
            //     else if (rooms.Count >= SmallNumberOfRoomsThreshold)
            //         StartupCacheFetchMode = StartupCacheFetchMode.Asynchronous;
            //     else
            //         StartupCacheFetchMode = StartupCacheFetchMode.Synchronous;
            // }

            ClientState state = new(rooms.Count, 0);
            foreach (Room room in rooms)
            {
                SocketRoom socketRoom = AddRoom(room, state);
                // if (StartupCacheFetchMode is StartupCacheFetchMode.Lazy)
                // {
                //     if (socketRoom.IsAvailable)
                //         await RoomAvailableAsync(socketRoom).ConfigureAwait(false);
                //     else
                //         await RoomUnavailableAsync(socketRoom).ConfigureAwait(false);
                // }
            }

            State = state;

            // if (StartupCacheFetchMode is StartupCacheFetchMode.Synchronous
            //     && state.Rooms.Count > LargeNumberOfRoomsThreshold)
            // {
            //     await _gatewayLogger
            //         .WarningAsync($"The client is in synchronous startup mode and has joined {state.Rooms.Count} rooms. "
            //             + "This may cause the client to take a long time to start up with blocking the gateway, "
            //             + "which may result in a timeout or socket disconnection. "
            //             + "Consider using asynchronous mode or lazy mode.").ConfigureAwait(false);
            // }

            // _roomDownloadTask = StartupCacheFetchMode is not StartupCacheFetchMode.Lazy
            //     ? DownloadRoomDataAsync(state.Rooms, Connection.CancellationToken)
            //     : Task.CompletedTask;
            _ = Connection.CompleteAsync();

            // if (StartupCacheFetchMode is StartupCacheFetchMode.Synchronous)
            //     await _roomDownloadTask.ConfigureAwait(false);

            await TimedInvokeAsync(_readyEvent, nameof(Ready)).ConfigureAwait(false);
            await _gatewayLogger.InfoAsync("Ready").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Connection.CriticalError(new Exception("Processing Rooms failed", ex));
            return;
        }
    }

    private async Task DownloadRoomDataAsync(IEnumerable<SocketRoom> socketRooms, CancellationToken cancellationToken)
    {
        try
        {
            await _gatewayLogger.DebugAsync("RoomDownloader Started").ConfigureAwait(false);

            // foreach (SocketRoom socketRoom in socketRooms)
            // {
            //     if (cancellationToken.IsCancellationRequested)
            //         break;
            //
            //     if (!socketRoom.IsAvailable)
            //         await socketRoom.UpdateAsync(new RequestOptions { CancellationToken = cancellationToken }).ConfigureAwait(false);
            //
            //     if (socketRoom.IsAvailable)
            //         await RoomAvailableAsync(socketRoom).ConfigureAwait(false);
            //     else
            //         await RoomUnavailableAsync(socketRoom).ConfigureAwait(false);
            // }
            //
            // await _gatewayLogger.DebugAsync("RoomDownloader Stopped").ConfigureAwait(false);
            //
            // // Download user list if enabled
            // if (BaseConfig.StartupCacheFetchData.HasFlag(StartupCacheFetchData.RoomUsers))
            // {
            //     _ = Task.Run(async () =>
            //     {
            //         try
            //         {
            //             IEnumerable<SocketRoom> availableRooms = Rooms
            //                 .Where(x => x.IsAvailable && x.HasAllMembers is not true);
            //             await DownloadUsersAsync(availableRooms, new RequestOptions
            //             {
            //                 CancellationToken = cancellationToken
            //             });
            //         }
            //         catch (Exception ex)
            //         {
            //             await _gatewayLogger.WarningAsync("Downloading users failed", ex).ConfigureAwait(false);
            //         }
            //     }, cancellationToken);
            // }
        }
        catch (OperationCanceledException)
        {
            await _gatewayLogger.DebugAsync("RoomDownloader Stopped").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _gatewayLogger.ErrorAsync("RoomDownloader Errored", ex).ConfigureAwait(false);
        }
    }

    #endregion

    #region Room Members

    private async Task HandleJoinedLeftRoom(JsonElement payload)
    {
        if (DeserializePayload<RoomMemberJoinLeftEvent>(payload) is not { } memberEvent) return;
        SocketRoom room = await GetOrCreateRoomAsync(State, memberEvent.RoomBaseInfo.RoomId, memberEvent.RoomBaseInfo);
        switch (memberEvent.State)
        {
            case RoomMemberAction.Join:
            {
                SocketRoomUser user = room.AddOrUpdateUser(memberEvent.UserInfo);
                // room.MemberCount++;
                if (memberEvent.UserInfo.UserId == CurrentUser?.Id)
                    await TimedInvokeAsync(_joinedRoomEvent, nameof(JoinedRoom), room).ConfigureAwait(false);
                else
                    await TimedInvokeAsync(_userJoinedEvent, nameof(UserJoined), user).ConfigureAwait(false);
                return;
            }
            case RoomMemberAction.Left:
            {
                SocketRoomUser user = room.RemoveUser(memberEvent.UserInfo.UserId)
                    ?? SocketRoomUser.Create(room, State, memberEvent.UserInfo);
                // room.MemberCount--;
                if (memberEvent.UserInfo.UserId == CurrentUser?.Id)
                {
                    SocketRoom? roomLeft = State.RemoveRoom(room.Id);
                    await TimedInvokeAsync(_leftRoomEvent, nameof(LeftRoom), roomLeft ?? room).ConfigureAwait(false);
                }
                else
                    await TimedInvokeAsync(_userLeftEvent, nameof(UserLeft), user).ConfigureAwait(false);
                return;
            }
            default:
                throw new NotSupportedException("Unsupported member action");
        }
    }

    #endregion

    #region Reactions

    private async Task HandleAddedRemovedReaction(JsonElement payload)
    {
        if (DeserializePayload<ReactionEvent>(payload) is not { } reactionEvent) return;
        SocketRoom room = await GetOrCreateRoomAsync(State, reactionEvent.RoomId, null);
        if (room.AddOrUpdateChannel(reactionEvent.ChannelId, ChannelType.Text) is not SocketTextChannel textChannel)
        {
            await _gatewayLogger.WarningAsync("Received a reaction event for a non-text channel").ConfigureAwait(false);
            return;
        }
        Cacheable<IUserMessage, ulong> cacheableMessage = CreateCacheableUserMessage(reactionEvent.MessageId);
        Cacheable<SocketTextChannel, ulong> cacheableChannel = CreateCacheableTextChannel(textChannel);
        Cacheable<SocketRoomUser, uint> cacheableUser = CreateCacheableRoomUser(reactionEvent.UserId);
        SocketReaction reaction = SocketReaction.Create(State, reactionEvent);
        switch (reactionEvent.Action)
        {
            case ReactionAction.Add:
                await TimedInvokeAsync(_reactionAddedEvent, nameof(ReactionAdded),
                        cacheableMessage, cacheableChannel, cacheableUser, reaction)
                    .ConfigureAwait(false);
                break;
            case ReactionAction.Remove:
                await TimedInvokeAsync(_reactionRemovedEvent, nameof(ReactionRemoved),
                        cacheableMessage, cacheableChannel, cacheableUser, reaction)
                    .ConfigureAwait(false);
                break;
            default:
                throw new NotSupportedException("Unsupported reaction operation");
        }
    }

    #endregion

    #region Interactions

    private async Task HandleSlashCommand(JsonElement payload)
    {
        if (DeserializePayload<CommandEvent>(payload) is not { } commandEvent) return;
        if (commandEvent.BotId != CurrentUser?.Id)
        {
            await _gatewayLogger.WarningAsync("Received a command event for a different bot").ConfigureAwait(false);
            return;
        }

        SocketRoom room = await GetOrCreateRoomAsync(State, commandEvent.RoomBaseInfo.RoomId, commandEvent.RoomBaseInfo);
        if (room.AddOrUpdateChannel(commandEvent.ChannelBaseInfo) is not SocketTextChannel channel)
        {
            await _gatewayLogger.WarningAsync("Received a command event for a non-text channel").ConfigureAwait(false);
            return;
        }
        SocketRoomUser user = room.AddOrUpdateUser(commandEvent.SenderInfo);
        SocketInteraction interaction = SocketInteraction.Create(this, commandEvent.CommandInfo, channel, user, commandEvent.MessageId);

        await TimedInvokeAsync(_interactionCreatedEvent, nameof(InteractionCreated), interaction).ConfigureAwait(false);
        switch (interaction)
        {
            case SocketSlashCommand slashCommand:
                await TimedInvokeAsync(_slashCommandExecutedEvent, nameof(SlashCommandExecuted), slashCommand).ConfigureAwait(false);
                break;
        }
    }

    private async Task HandleCardMessageButtonClick(ulong sequence, JsonElement payload)
    {
        if (DeserializePayload<CardMessageButtonClickEvent>(payload) is not { } clickEvent) return;
        SocketRoom room = await GetOrCreateRoomAsync(State, clickEvent.RoomBaseInfo.RoomId, clickEvent.RoomBaseInfo);
        if (room.AddOrUpdateChannel(clickEvent.ChannelBaseInfo) is not SocketTextChannel channel)
        {
            await _gatewayLogger.WarningAsync("Received a command event for a non-text channel").ConfigureAwait(false);
            return;
        }
        SocketRoomUser user = room.AddOrUpdateUser(clickEvent.SenderInfo);
        SocketInteraction interaction = SocketInteraction.Create(this, clickEvent, channel, user, sequence);

        await TimedInvokeAsync(_interactionCreatedEvent, nameof(InteractionCreated), interaction).ConfigureAwait(false);
        if (interaction is SocketButtonClick buttonClicked)
            await TimedInvokeAsync(_buttonClickedEvent, nameof(ButtonClicked), buttonClicked).ConfigureAwait(false);
    }

    #endregion

    #region Cacheable

    private Cacheable<IUserMessage, ulong> CreateCacheableUserMessage(ulong id) =>
        new(null, id, false, () => Task.FromResult<IUserMessage?>(null));

    private Cacheable<SocketTextChannel, ulong> CreateCacheableTextChannel(SocketTextChannel channel) =>
        new(channel, channel.Id, true, () => Task.FromResult<SocketTextChannel?>(channel));

    private Cacheable<SocketTextChannel, ulong> CreateCacheableTextChannel(ulong id) =>
        new(null, id, false, () => Task.FromResult<SocketTextChannel?>(null));

    private Cacheable<SocketRoomUser, uint> CreateCacheableRoomUser(uint id) =>
        new(null, id, false, () => Task.FromResult<SocketRoomUser?>(null));

    #endregion
}
