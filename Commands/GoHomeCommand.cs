namespace TeleWoL.Commands;

internal class GoHomeCommand : CommandBase
{
    private GoHomeCommand()
    {
        Text = "> Go home";
    }

    public static readonly GoHomeCommand Instance = new GoHomeCommand();
}