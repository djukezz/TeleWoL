using TeleWoL.States;

namespace TeleWoL.Commands;

internal class GoHomeCommand : StateCommandBase<MainState>
{
    private GoHomeCommand()
    {
        Text = "> Go home";
    }

    public static readonly GoHomeCommand Instance = new GoHomeCommand();
}