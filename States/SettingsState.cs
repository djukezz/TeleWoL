using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal sealed class SettingsState : StateBase
{
    public override void Init()
    {
        _commands.Add(AddTargetsCommand.Instance);
        _commands.Add(DeleteTargetsCommand.Instance);
        if (UserSettings.Permission == UserPermission.Admin)
        {
            _commands.Add(AddUserCommand.Instance);
            _commands.Add(AddAdminCommand.Instance);
            _commands.Add(DeleteUsersCommand.Instance);
        }
        _commands.Add(GoHomeCommand.Instance);
    }

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is IStateCommand stateCommand)
        {
            newState = stateCommand.Execute(StatesFactory);
            return null;
        }

        return Response.Unknown;
    }
}
