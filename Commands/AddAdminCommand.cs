using TeleWoL.Settings;
using TeleWoL.States;

namespace TeleWoL.Commands;

internal sealed class AddAdminCommand : AddUserCommandBase<AddAdminState>
{
    private AddAdminCommand()
        : base("> Add admin", UserPermission.Admin)
    {
    }

    public static readonly AddAdminCommand Instance = new AddAdminCommand();
}
