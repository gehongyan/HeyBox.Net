using System.Diagnostics;
using Model = HeyBox.API.Role;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的身份组。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketRole : SocketEntity<ulong>, IRole
{
    /// <inheritdoc cref="HeyBox.IRole.Room" />
    public SocketRoom Room { get; }

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

    internal SocketRole(SocketRoom room, ulong id)
        : base(room.Client, id)
    {
        Room = room;
        Name = string.Empty;
        Icon = string.Empty;
    }

    internal static SocketRole Create(SocketRoom room, ClientState state, Model model)
    {
        SocketRole entity = new(room, model.Id);
        entity.Update(state, model);
        return entity;
    }

    internal void Update(ClientState state, Model model)
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

        IsPopulated = true;
    }

    /// <inheritdoc cref="HeyBox.WebSocket.SocketRole.Name" />
    public override string ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id}, {Type})";

    #region IRole

    /// <inheritdoc />
    IRoom IRole.Room => Room;

    #endregion
}
