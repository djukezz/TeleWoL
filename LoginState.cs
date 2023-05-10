namespace StateTest;

internal sealed class LoginState : StateBase
{
    public LoginState()
    {
    }

    public override string Name => "Login";

    public override string Description => "Please enter password";

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is StringCommand cmd)
        {
            if (cmd.Text == "1234")
            {
                newState = new IdleState(new UserSettings());
                return "Hello!";
            }
        }
        return "Wrong password!";
    }

    protected override CommandBase? ConvertCommand(string command) =>
        string.IsNullOrWhiteSpace(command) ? null : new StringCommand(command);
}
