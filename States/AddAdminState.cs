using TeleWoL.Settings;

namespace TeleWoL.States;

internal sealed class AddAdminState : AddUserStateBase
{
    protected override UserPermission GetPermission() => UserPermission.Admin;
}