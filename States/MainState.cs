using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal class MainState : StateBase
{
    public override void Init()
    {
        _commands.AddRange(UserSettings.GetTargets()
            .Select(t => new TurnOnCommand(t)));

        _commands.Add(SettingsCommand.Instance);
    }

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is TurnOnCommand turnOnCommand)
            return turnOnCommand.Execute();

        if (command is IStateCommand stateCommand)
        {
            newState = stateCommand.Execute(StatesFactory);
            return null;
        }

        return Response.Unknown;
    }
}
