using TeleWoL.Settings;

namespace TeleWoL.States;

internal sealed class AddUserState : AddUserStateBase
{
    protected override UserPermission GetPermission() => UserPermission.User;
}
