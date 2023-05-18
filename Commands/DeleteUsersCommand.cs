using TeleWoL.States;

namespace TeleWoL.Commands;

internal class DeleteUsersCommand : StateCommandBase<DeleteUsersState>
{
    private DeleteUsersCommand()
    {
        Text = "> Delete user";
    }

    public static readonly DeleteUsersCommand Instance = new DeleteUsersCommand();
}