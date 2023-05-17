using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal sealed class SettingsState : UserStateBase
{
    public SettingsState(UserSettings settings)
        : base(settings)
    {
        _commands.Add(SettingsAddCommand.Instance);
        _commands.Add(SettingsDeleteCommand.Instance);
        _commands.Add(GoHomeCommand.Instance);
    }

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is GoHomeCommand)
            newState = new MainState(_settings);
        else if(command is SettingsDeleteCommand)
            newState = new DeleteTargetState(_settings);
        else if (command is SettingsAddCommand)
            newState = new AddTargetState(_settings);

        return null;
    }
}
