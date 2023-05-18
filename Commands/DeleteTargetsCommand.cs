using TeleWoL.States;

namespace TeleWoL.Commands;

internal class DeleteTargetsCommand : StateCommandBase<DeleteTargetsState>
{
    private DeleteTargetsCommand()
    {
        Text = "> Delete target";
    }

    public static readonly DeleteTargetsCommand Instance = new DeleteTargetsCommand();
}
