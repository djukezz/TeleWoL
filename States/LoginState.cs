using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal sealed class LoginState : StateBase
{
    private readonly GlobalSettings _settings;
    private readonly UserContext _userContext;

    public LoginState(GlobalSettings settings, UserContext userContext)
    {
        _settings = settings;
        _userContext = userContext;
    }

    public override string Description => "Please enter password";

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is StringCommand cmd)
        {
            UserPermission? permission = cmd.Text == _settings.AdminPassword ?
                UserPermission.Admin :
                cmd.Text == _settings.UserPassword ?
                    UserPermission.User :
                    null;

            if (permission != null)
            {
                var userSettings = _settings.GetOrAdd(_userContext.UserId);

                userSettings.Permission = permission.Value;
                userSettings.UserName = _userContext.UserName;
                newState = new MainState(userSettings);
                return "Hello!";
            }
        }
        return "Wrong password!";
    }

    protected override CommandBase? ConvertCommand(string command) =>
        string.IsNullOrWhiteSpace(command) ? null : new StringCommand(command);
}
