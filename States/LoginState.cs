using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal sealed class LoginState : StateBase
{
    public override string Description => "Please enter password";

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is StringCommand cmd)
        {
            UserPermission permission = GlobalSettings.GetPermission(cmd.Text);

            if (permission != UserPermission.None)
            {
                UserSettings.Permission = permission;
                UserSettings.UserName = UserContext.UserName;
                newState = StatesFactory.Create<MainState>();
                FireSettingsChanged();
                return $"Hello {permission}!";
            }
        }
        return "Wrong password!";
    }

    protected override CommandBase? ConvertCommand(string command) =>
        string.IsNullOrWhiteSpace(command) ? null : new StringCommand(command);
}
