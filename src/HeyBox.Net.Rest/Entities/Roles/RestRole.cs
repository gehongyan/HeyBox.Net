using Model = HeyBox.API.Role;

namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的身份组。
/// </summary>
public class RestRole : RestEntity<ulong>, IRole
{
    /// <inheritdoc />
    public IRoom Room { get; }

    /// <inheritdoc />
    public RoleType Type { get; internal set; }

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public string Icon { get; private set; }

    /// <inheritdoc />
    public Color Color { get; private set; }

    /// <inheritdoc />
    public ColorType ColorType { get; private set; }

    /// <inheritdoc />
    public GradientColor? GradientColor { get; private set; }

    /// <inheritdoc />
    public int Position { get; private set; }

    /// <inheritdoc />
    public bool IsHoisted { get; private set; }

    /// <inheritdoc />
    public bool IsMentionable { get; private set; }

    /// <inheritdoc />
    public RoomPermissions Permissions { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; private set; }

    /// <inheritdoc />
    public ulong CreatorId { get; private set; }

    /// <summary>
    ///     获取此角色是否为 <c>@全体成员</c> 全体成员角色。
    /// </summary>
    public bool IsEveryone => Id == 0;

    /// <inheritdoc />
    public string Mention => IsEveryone ? "@{all}" : MentionUtils.MentionRole(Id);

    internal RestRole(BaseHeyBoxClient client, IRoom room, ulong id)
        : base(client, id)
    {
        Room = room;
        Name = string.Empty;
        Icon = string.Empty;
    }

    internal static RestRole Create(BaseHeyBoxClient client, IRoom room, Model model)
    {
        RestRole entity = new(client, room, model.Id);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
    {
        Type = model.Type;
        Name = model.Name;
        Icon = model.Icon;
        ColorType = model.GradientColor.HasValue ? ColorType.Gradient : ColorType.Solid;
        Color = model.Color;
        GradientColor = model.GradientColor;
        Permissions = new RoomPermissions(model.Permissions);
        Position = model.Position;
        IsHoisted = model.Hoist;
        IsMentionable = model.Mentionable;
        CreatedAt = model.CreateTime;
        CreatorId = model.Creator;
    }

    /// <inheritdoc />
    public async Task ModifyAsync(Action<RoleProperties> func, RequestOptions? options = null)
    {
        Model model = await RoleHelper.ModifyAsync(this, Client, func, options);
        Update(model);
    }

    /// <inheritdoc />
    public Task DeleteAsync(RequestOptions? options = null) =>
        RoleHelper.DeleteAsync(this, Client, options);
}
