using System.Text.Json;
using HeyBox.API.Gateway;

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

        SocketRoom room = GetOrCreateRoom(State, commandEvent.RoomBaseInfo);
        if (room.AddOrUpdateChannel(commandEvent.ChannelBaseInfo) is not SocketTextChannel channel)
        {
            await _gatewayLogger.WarningAsync("Received a command event for a non-text channel").ConfigureAwait(false);
            return;
        }
        SocketRoomUser user = room.AddOrUpdateUser(commandEvent.SenderInfo);
        SocketInteraction interaction = SocketInteraction.Create(this, commandEvent.CommandInfo, channel, user);

        await TimedInvokeAsync(_interactionCreatedEvent, nameof(InteractionCreated), interaction).ConfigureAwait(false);
        switch (interaction)
        {
            case SocketSlashCommand slashCommand:
                await TimedInvokeAsync(_slashCommandExecuted, nameof(SlashCommandExecuted), slashCommand).ConfigureAwait(false);
                break;
        }
    }

    #endregion
}
