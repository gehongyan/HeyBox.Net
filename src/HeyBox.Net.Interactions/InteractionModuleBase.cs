namespace HeyBox.Interactions;

/// <summary>
///     提供一个用于继承的命令模块基类。
/// </summary>
/// <typeparam name="T"> 命令上下文的类型。 </typeparam>
public abstract class InteractionModuleBase<T> : IInteractionModuleBase where T : class, IInteractionContext
{
    /// <summary>
    ///     Gets the underlying context of the command.
    /// </summary>
    public T Context { get; private set; } = null!; // Will be set immediately after construction

    /// <inheritdoc/>
    public virtual void AfterExecute(ICommandInfo command) { }

    /// <inheritdoc/>
    public virtual void BeforeExecute(ICommandInfo command) { }

    /// <inheritdoc/>
    public virtual Task BeforeExecuteAsync(ICommandInfo command) => Task.CompletedTask;

    /// <inheritdoc/>
    public virtual Task AfterExecuteAsync(ICommandInfo command) => Task.CompletedTask;

    /// <inheritdoc/>
    public virtual void OnModuleBuilding(InteractionService commandService, ModuleInfo module) { }

    /// <inheritdoc/>
    public virtual void Construct(Builders.ModuleBuilder builder, InteractionService commandService) { }

    internal void SetContext(IInteractionContext context)
    {
        if (context is not T typedContext)
            throw new InvalidOperationException($"Invalid context type. Expected {typeof(T).Name}, got {context.GetType().Name}.");
        Context = typedContext;
    }

    // /// <inheritdoc cref="IHeyBoxInteraction.DeferAsync(bool, RequestOptions)"/>
    // protected virtual Task DeferAsync(bool ephemeral = false, RequestOptions options = null)
    //     => Context.Interaction.DeferAsync(ephemeral, options);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.RespondAsync(string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task RespondAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, RequestOptions options = null, MessageComponent components = null, Embed embed = null, PollProperties poll = null)
    //     => Context.Interaction.RespondAsync(text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.RespondWithFileAsync(Stream, string, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task RespondWithFileAsync(Stream fileStream, string fileName, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null, PollProperties poll = null)
    //     => Context.Interaction.RespondWithFileAsync(fileStream, fileName, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.RespondWithFileAsync(string, string, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task RespondWithFileAsync(string filePath, string fileName = null, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null, PollProperties poll = null)
    //     => Context.Interaction.RespondWithFileAsync(filePath, fileName, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.RespondWithFileAsync(FileAttachment, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task RespondWithFileAsync(FileAttachment attachment, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null, PollProperties poll = null)
    //     => Context.Interaction.RespondWithFileAsync(attachment, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.RespondWithFilesAsync(IEnumerable{FileAttachment}, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task RespondWithFilesAsync(IEnumerable<FileAttachment> attachments, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null, PollProperties poll = null)
    //     => Context.Interaction.RespondWithFilesAsync(attachments, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.FollowupAsync(string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task<IUserMessage> FollowupAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, RequestOptions options = null, MessageComponent components = null, Embed embed = null, PollProperties poll = null)
    //     => Context.Interaction.FollowupAsync(text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.FollowupWithFileAsync(Stream, string, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task<IUserMessage> FollowupWithFileAsync(Stream fileStream, string fileName, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null, PollProperties poll = null)
    //     => Context.Interaction.FollowupWithFileAsync(fileStream, fileName, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.FollowupWithFileAsync(string, string, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task<IUserMessage> FollowupWithFileAsync(string filePath, string fileName = null, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null, PollProperties poll = null)
    //     => Context.Interaction.FollowupWithFileAsync(filePath, fileName, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.FollowupWithFileAsync(FileAttachment, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task<IUserMessage> FollowupWithFileAsync(FileAttachment attachment, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null, PollProperties poll = null)
    //     => Context.Interaction.FollowupWithFileAsync(attachment, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.FollowupWithFilesAsync(IEnumerable{FileAttachment}, string, Embed[], bool, bool, AllowedMentions, MessageComponent, Embed, RequestOptions, PollProperties)"/>
    // protected virtual Task<IUserMessage> FollowupWithFilesAsync(IEnumerable<FileAttachment> attachments, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false,
    //     AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null, PollProperties poll = null)
    //     => Context.Interaction.FollowupWithFilesAsync(attachments, text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options, poll);
    //
    // /// <inheritdoc cref="IMessageChannel.SendMessageAsync(string, bool, Embed, RequestOptions, AllowedMentions, MessageReference, MessageComponent, ISticker[], Embed[], MessageFlags, PollProperties)"/>
    // protected virtual Task<IUserMessage> ReplyAsync(string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null,
    //     AllowedMentions allowedMentions = null, MessageReference messageReference = null, MessageComponent components = null, ISticker[] stickers = null,
    //     Embed[] embeds = null, MessageFlags flags = MessageFlags.None, PollProperties poll = null)
    //     => Context.Channel.SendMessageAsync(text, false, embed, options, allowedMentions, messageReference, components, stickers, embeds, flags, poll);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.GetOriginalResponseAsync(RequestOptions)"/>
    // protected virtual Task<IUserMessage> GetOriginalResponseAsync(RequestOptions options = null)
    //     => Context.Interaction.GetOriginalResponseAsync(options);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.ModifyOriginalResponseAsync(Action{MessageProperties}, RequestOptions)"/>
    // protected virtual Task<IUserMessage> ModifyOriginalResponseAsync(Action<MessageProperties> func, RequestOptions options = null)
    //     => Context.Interaction.ModifyOriginalResponseAsync(func, options);
    //
    // /// <inheritdoc cref="IDeletable.DeleteAsync(RequestOptions)"/>
    // protected virtual Task DeleteOriginalResponseAsync()
    //     => Context.Interaction.DeleteOriginalResponseAsync();
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.RespondWithModalAsync(Modal, RequestOptions)"/>
    // protected virtual Task RespondWithModalAsync(Modal modal, RequestOptions options = null)
    //     => Context.Interaction.RespondWithModalAsync(modal, options);
    //
    // /// <inheritdoc cref="IHeyBoxInteractionExtentions.RespondWithModalAsync{T}(IHeyBoxInteraction, string, T, RequestOptions, Action{ModalBuilder})"/>
    // protected virtual Task RespondWithModalAsync<TModal>(string customId, TModal modal, RequestOptions options = null, Action<ModalBuilder> modifyModal = null) where TModal : class, IModal
    //     => Context.Interaction.RespondWithModalAsync(customId, modal, options, modifyModal);
    //
    // /// <inheritdoc cref="IHeyBoxInteractionExtentions.RespondWithModalAsync{T}(IHeyBoxInteraction, string, RequestOptions, Action{ModalBuilder})"/>
    // protected virtual Task RespondWithModalAsync<TModal>(string customId, RequestOptions options = null, Action<ModalBuilder> modifyModal = null) where TModal : class, IModal
    //     => Context.Interaction.RespondWithModalAsync<TModal>(customId, options, modifyModal);
    //
    // /// <inheritdoc cref="IHeyBoxInteraction.RespondWithPremiumRequiredAsync(RequestOptions)"/>
    // protected virtual Task RespondWithPremiumRequiredAsync(RequestOptions options = null)
    //     => Context.Interaction.RespondWithPremiumRequiredAsync(options);

    //IInteractionModuleBase

    /// <inheritdoc/>
    void IInteractionModuleBase.SetContext(IInteractionContext context) => SetContext(context);
}

/// <summary>
///     提供一个用于继承的命令模块基类。
/// </summary>
public abstract class InteractionModuleBase : InteractionModuleBase<IInteractionContext> { }