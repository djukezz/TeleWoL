using TeleWoL.Commands;
using TeleWoL.Settings;
using TeleWoL.WakeOnLan;

namespace TeleWoL.States;

internal class MainState : StateBase
{
    private readonly IWoLSender _woLSender;

    public MainState(IWoLSender woLSender)
    {
        _woLSender = woLSender;
    }

    public override void Init()
    {
        _commands.AddRange(UserSettings.GetTargets()
            .Select(t => new TurnOnCommand(t, _woLSender)));

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
