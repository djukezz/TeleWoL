namespace StateTest;

internal sealed class SettingsState : UserStateBase
{
    public SettingsState(UserSettings settings)
        : base(settings)
    {
        _commands.Add(new GoHomeCommand());
    }

    public override string Name => "Settings";

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is GoHomeCommand goHomeCommand)
            newState = new IdleState(_settings);
        return null;
    }
}
