namespace StateTest;

internal class GoHomeCommand : CommandBase
{
    public GoHomeCommand()
    {
        Text = "Go home";
        Id = "GoHome";
    }
}