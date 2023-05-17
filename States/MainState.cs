using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal class MainState : UserStateBase
{
    public MainState(UserSettings settings) : base(settings)
    {
        _commands.AddRange(settings.GetTargets()
            .Select(t => new TurnOnCommand(t)));
        _commands.Add(SettingsCommand.Instance);
    }

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is TurnOnCommand turnOnCommand)
            return turnOnCommand.Execute();

        if (command is SettingsCommand)
        {
            newState = new SettingsState(_settings);
            return null;
        }

        return Response.Unknown;
    }
}
