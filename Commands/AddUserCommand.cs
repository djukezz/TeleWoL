using TeleWoL.Settings;
using TeleWoL.States;

namespace TeleWoL.Commands;

internal sealed class AddUserCommand : AddUserCommandBase<AddUserState>
{
    private AddUserCommand()
        : base("> Add user", UserPermission.User)
    {
    }

    public static readonly AddUserCommand Instance = new AddUserCommand();
}
