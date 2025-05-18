namespace HeyBox;

/// <summary>
///     提供用于 <see cref="INode"/>、<see cref="IModule"/> 和 <see cref="ICard"/> 等卡片相关对象的扩展方法。
/// </summary>
public static class CardExtensions
{
    #region Nodes

    /// <summary>
    ///     将 <see cref="INode"/> 实体转换为具有相同属性的 <see cref="INodeBuilder"/> 实体构建器。
    /// </summary>
    public static INodeBuilder ToBuilder(this INode entity)
    {
        return entity switch
        {
            PlainTextNode { Type: NodeType.PlainText } plainTextNode => plainTextNode.ToBuilder(),
            MarkdownNode { Type: NodeType.Markdown } markdownNode => markdownNode.ToBuilder(),
            ButtonNode { Type: NodeType.Button } buttonNode => buttonNode.ToBuilder(),
            ImageNode { Type: NodeType.Image } imageNode => imageNode.ToBuilder(),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        };
    }

    /// <summary>
    ///     将 <see cref="PlainTextNode"/> 实体转换为具有相同属性的 <see cref="PlainTextNodeBuilder"/> 实体构建器。
    /// </summary>
    public static PlainTextNodeBuilder ToBuilder(this PlainTextNode entity)
    {
        return new PlainTextNodeBuilder { Text = entity.Text };
    }

    /// <summary>
    ///     将 <see cref="MarkdownNode"/> 实体转换为具有相同属性的 <see cref="MarkdownNodeBuilder"/> 实体构建器。
    /// </summary>
    public static MarkdownNodeBuilder ToBuilder(this MarkdownNode entity)
    {
        return new MarkdownNodeBuilder { Text = entity.Text };
    }

    /// <summary>
    ///     将 <see cref="ButtonNode"/> 实体转换为具有相同属性的 <see cref="ButtonNodeBuilder"/> 实体构建器。
    /// </summary>
    public static ButtonNodeBuilder ToBuilder(this ButtonNode entity)
    {
        return new ButtonNodeBuilder
        {
            Theme = entity.Theme,
            Event = entity.Event,
            Value = entity.Value,
            Text = entity.Text
        };
    }

    /// <summary>
    ///     将 <see cref="ImageNode"/> 实体转换为具有相同属性的 <see cref="ImageNodeBuilder"/> 实体构建器。
    /// </summary>
    public static ImageNodeBuilder ToBuilder(this ImageNode entity)
    {
        return new ImageNodeBuilder
        {
            Url = entity.Url,
            Size = entity.Size
        };
    }

    #endregion

    #region Modules

    /// <summary>
    ///     将 <see cref="IModule"/> 实体转换为具有相同属性的 <see cref="IModuleBuilder"/> 实体构建器。
    /// </summary>
    public static IModuleBuilder ToBuilder(this IModule entity)
    {
        return entity switch
        {
            SectionModule { Type: ModuleType.Section } sectionModule => sectionModule.ToBuilder(),
            HeaderModule { Type: ModuleType.Header } headerModule => headerModule.ToBuilder(),
            ImagesModule { Type: ModuleType.Images } imagesModule => imagesModule.ToBuilder(),
            ButtonGroupModule { Type: ModuleType.ButtonGroup } buttonGroupModule => buttonGroupModule.ToBuilder(),
            DividerModule { Type: ModuleType.Divider } dividerModule => dividerModule.ToBuilder(),
            CountdownModule { Type: ModuleType.Countdown } countdownModule => countdownModule.ToBuilder(),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        };
    }

    /// <summary>
    ///     将 <see cref="SectionModule"/> 实体转换为具有相同属性的 <see cref="SectionModuleBuilder"/> 实体构建器。
    /// </summary>
    public static SectionModuleBuilder ToBuilder(this SectionModule entity)
    {
        return new SectionModuleBuilder
        {
            Paragraph = entity.Paragraph.Select(x => x.ToBuilder()).ToList(),
        };
    }

    /// <summary>
    ///     将 <see cref="HeaderModule"/> 实体转换为具有相同属性的 <see cref="HeaderModuleBuilder"/> 实体构建器。
    /// </summary>
    public static HeaderModuleBuilder ToBuilder(this HeaderModule entity)
    {
        return new HeaderModuleBuilder
        {
            Content = entity.Content?.ToBuilder() as ITextNodeBuilder
        };
    }

    /// <summary>
    ///     将 <see cref="ImagesModule"/> 实体转换为具有相同属性的 <see cref="ImagesModuleBuilder"/> 实体构建器。
    /// </summary>
    public static ImagesModuleBuilder ToBuilder(this ImagesModule entity)
    {
        return new ImagesModuleBuilder
        {
            Images = entity.Images.Select(x => x.ToBuilder()).ToList()
        };
    }

    /// <summary>
    ///     将 <see cref="ButtonGroupModule"/> 实体转换为具有相同属性的 <see cref="ButtonGroupModuleBuilder"/> 实体构建器。
    /// </summary>
    public static ButtonGroupModuleBuilder ToBuilder(this ButtonGroupModule entity)
    {
        return new ButtonGroupModuleBuilder
        {
            Buttons = entity.Buttons.Select(x => x.ToBuilder()).ToList()
        };
    }

    /// <summary>
    ///     将 <see cref="DividerModule"/> 实体转换为具有相同属性的 <see cref="DividerModuleBuilder"/> 实体构建器。
    /// </summary>
    public static DividerModuleBuilder ToBuilder(this DividerModule _)
    {
        return new DividerModuleBuilder();
    }

    /// <summary>
    ///     将 <see cref="CountdownModule"/> 实体转换为具有相同属性的 <see cref="CountdownModuleBuilder"/> 实体构建器。
    /// </summary>
    public static CountdownModuleBuilder ToBuilder(this CountdownModule entity)
    {
        return new CountdownModuleBuilder
        {
            Mode = entity.Mode,
            EndTime = entity.EndTime
        };
    }

    #endregion

    #region Cards

    /// <summary>
    ///     将 <see cref="ICard"/> 实体转换为具有相同属性的 <see cref="ICardBuilder"/> 实体构建器。
    /// </summary>
    public static ICardBuilder ToBuilder(this ICard entity)
    {
        return entity switch
        {
            Card { Type: CardType.Card } card => card.ToBuilder(),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        };
    }

    /// <summary>
    ///     将 <see cref="Card"/> 实体转换为具有相同属性的 <see cref="CardBuilder"/> 实体构建器。
    /// </summary>
    public static CardBuilder ToBuilder(this Card entity)
    {
        return new CardBuilder
        {
            Modules = entity.Modules.Select(m => m.ToBuilder()).ToList()
        };
    }

    #endregion
}
