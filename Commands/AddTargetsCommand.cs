using TeleWoL.States;

namespace TeleWoL.Commands;

internal class AddTargetsCommand : StateCommandBase<AddTargetState>
{
    private AddTargetsCommand()
    {
        Text = "> Add target";
    }

    public static readonly AddTargetsCommand Instance = new AddTargetsCommand();
}