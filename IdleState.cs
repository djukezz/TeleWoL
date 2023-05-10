namespace StateTest;

internal class IdleState : UserStateBase
{
    public IdleState(UserSettings settings) : base(settings)
    {
        _commands.AddRange(settings.Targets
            .Select(t => new TurnOnCommand(t)));
    }

    public override string Name => "Idle";

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is TurnOnCommand turnOnCommand)
            return turnOnCommand.Execute();

        if (command is SettingsCommand)
            newState = new SettingsState(_settings);

        return Response.Unknown;
    }
}
