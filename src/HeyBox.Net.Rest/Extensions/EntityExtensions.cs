namespace HeyBox.Rest;

internal static class EntityExtensions
{
    #region Emotes

    public static RoomEmote ToEmoteEntity(this API.Meme model, IRoomUser creator) =>
        new(model.Name, model.Path, creator.Room, creator, model.Extension, model.CreateTime);

    public static RoomSticker ToStickerEntity(this API.Meme model, IRoomUser creator) =>
        new(model.Name, model.Path, creator.Room, creator, model.Extension, model.CreateTime);

    #endregion

    #region Nodes

    public static INode ToEntity(this API.NodeBase model)
    {
        return model switch
        {
            API.PlainTextNode { Type: NodeType.PlainText } x => x.ToEntity(),
            API.MarkdownNode { Type: NodeType.Markdown } x => x.ToEntity(),
            API.ImageNode { Type: NodeType.Image } x => x.ToEntity(),
            API.ButtonNode { Type: NodeType.Button } x => x.ToEntity(),
            _ => throw new ArgumentOutOfRangeException(nameof(model))
        };
    }

    public static PlainTextNode ToEntity(this API.PlainTextNode model) => new(
        model.Text, !string.IsNullOrWhiteSpace(model.Width) ? NodeWidth.FromValue(model.Width) : null);

    public static MarkdownNode ToEntity(this API.MarkdownNode model) => new(
        model.Text, !string.IsNullOrWhiteSpace(model.Width) ? NodeWidth.FromValue(model.Width) : null);

    public static ImageNode ToEntity(this API.ImageNode model) => new(model.Url, model.Size,
        !string.IsNullOrWhiteSpace(model.Width) ? NodeWidth.FromValue(model.Width) : null);

    public static ButtonNode ToEntity(this API.ButtonNode model) => new(
        model.Text, model.Event, model.Value, model.Theme,
        !string.IsNullOrWhiteSpace(model.Width) ? NodeWidth.FromValue(model.Width) : null);

    public static API.NodeBase ToModel(this INode entity)
    {
        return entity switch
        {
            PlainTextNode { Type: NodeType.PlainText } x => x.ToModel(),
            MarkdownNode { Type: NodeType.Markdown } x => x.ToModel(),
            ImageNode { Type: NodeType.Image } x => x.ToModel(),
            ButtonNode { Type: NodeType.Button } x => x.ToModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        };
    }

    public static API.PlainTextNode ToModel(this PlainTextNode entity) => new()
    {
        Type = entity.Type,
        Text = entity.Text,
        Width = entity.Width?.Value
    };

    public static API.MarkdownNode ToModel(this MarkdownNode entity) => new()
    {
        Type = entity.Type,
        Text = entity.Text,
        Width = entity.Width?.Value
    };

    public static API.ImageNode ToModel(this ImageNode entity) => new()
    {
        Type = entity.Type,
        Url = entity.Url,
        Size = entity.Size,
        Width = entity.Width?.Value
    };

    public static API.ButtonNode ToModel(this ButtonNode entity) => new()
    {
        Type = entity.Type,
        Theme = entity.Theme,
        Value = entity.Value,
        Event = entity.Event,
        Text = entity.Text,
        Width = entity.Width?.Value
    };

    #endregion

    #region Modules

    public static IModule ToEntity(this API.ModuleBase model)
    {
        return model switch
        {
            API.SectionModule { Type: ModuleType.Section } x => x.ToEntity(),
            API.HeaderModule { Type: ModuleType.Header } x => x.ToEntity(),
            API.ImagesModule { Type: ModuleType.Images } x => x.ToEntity(),
            API.ButtonGroupModule { Type: ModuleType.ButtonGroup } x => x.ToEntity(),
            API.DividerModule { Type: ModuleType.Divider } x => x.ToEntity(),
            API.CountdownModule { Type: ModuleType.Countdown } x => x.ToEntity(),
            _ => throw new ArgumentOutOfRangeException(nameof(model))
        };
    }

    public static SectionModule ToEntity(this API.SectionModule model) =>
        new([..model.Paragraph.Select(x => x.ToEntity())]);

    public static HeaderModule ToEntity(this API.HeaderModule model) =>
        model.Content.ToEntity() is ITextNode textNode
            ? new HeaderModule(textNode)
            : throw new InvalidOperationException("Invalid text node.");

    public static ImagesModule ToEntity(this API.ImagesModule model) =>
        new([..model.Urls.Select(x => x.ToEntity())]);

    public static ButtonGroupModule ToEntity(this API.ButtonGroupModule model) =>
        new([..model.Buttons.Select(x => x.ToEntity())]);

    public static DividerModule ToEntity(this API.DividerModule model) => new(model.Text);

    public static CountdownModule ToEntity(this API.CountdownModule model) => new(model.Mode, model.EndTime);

    public static API.ModuleBase ToModel(this IModule entity)
    {
        return entity switch
        {
            SectionModule { Type: ModuleType.Section } x => x.ToModel(),
            HeaderModule { Type: ModuleType.Header } x => x.ToModel(),
            ImagesModule { Type: ModuleType.Images } x => x.ToModel(),
            ButtonGroupModule { Type: ModuleType.ButtonGroup } x => x.ToModel(),
            DividerModule { Type: ModuleType.Divider } x => x.ToModel(),
            CountdownModule { Type: ModuleType.Countdown } x => x.ToModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        };
    }

    public static API.SectionModule ToModel(this SectionModule entity) => new()
    {
        Type = entity.Type,
        Paragraph = [..entity.Paragraph.Select(x => x.ToModel())]
    };

    public static API.HeaderModule ToModel(this HeaderModule entity) => new()
    {
        Type = entity.Type,
        Content = entity.Content.ToModel()
    };

    public static API.ImagesModule ToModel(this ImagesModule entity) => new()
    {
        Type = entity.Type,
        Urls = [..entity.Images.Select(x => x.ToModel())]
    };

    public static API.ButtonGroupModule ToModel(this ButtonGroupModule entity) => new()
    {
        Type = entity.Type,
        Buttons = [..entity.Buttons.Select(x => x.ToModel())]
    };

    public static API.DividerModule ToModel(this DividerModule entity) => new()
    {
        Type = entity.Type,
        Text = entity.Text
    };

    public static API.CountdownModule ToModel(this CountdownModule entity) => new()
    {
        Type = entity.Type,
        Mode = entity.Mode,
        EndTime = entity.EndTime
    };

    #endregion

    #region Cards

    public static ICard ToEntity(this API.CardBase model) => model switch
    {
        API.Card x => x.ToEntity(),
        _ => throw new ArgumentOutOfRangeException(nameof(model))
    };

    public static Card ToEntity(this API.Card model) => new(
        !string.IsNullOrWhiteSpace(model.BorderColor) ? CssColor.FromValue(model.BorderColor) : null,
        model.Size ?? CardSize.Medium,
        [..model.Modules.Select(m => m.ToEntity())]);

    public static API.CardBase ToModel(this ICard entity) => entity switch
    {
        Card x => x.ToModel(),
        _ => throw new ArgumentOutOfRangeException(nameof(entity))
    };

    public static API.Card ToModel(this Card entity) => new()
    {
        Type = entity.Type,
        BorderColor = entity.Color?.Value,
        Size = entity.Size,
        Modules = [..entity.Modules.Select(m => m.ToModel())]
    };

    #endregion
}
