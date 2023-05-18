using TeleWoL.Settings;
using TeleWoL.States;

namespace TeleWoL.Commands;

internal abstract class AddUserCommandBase<T> : StateCommandBase<T>
    where T : StateBase
{
    protected AddUserCommandBase(string text, UserPermission permission)
    {
        Text = text;
        Permission = permission;
    }

    public UserPermission Permission { get; }
}
