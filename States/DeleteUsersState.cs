using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal class DeleteUsersState : StateBase
{
    public override void Init()
    {
        Update();
    }

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is DeleteUserCommand delete)
        {
            delete.Execute();
            Update();
            return null;
        }
        if (command is IStateCommand stateCommand)
        {
            newState = stateCommand.Execute(StatesFactory);
            return null;
        }

        return Response.Unknown;
    }

    private void Update()
    {
        _commands.Clear();
        _commands.AddRange(GlobalSettings.GetAll().Where(s=>s.UserId != UserSettings.UserId)
            .Select(t => new DeleteUserCommand(GlobalSettings, t)));
        _commands.Add(GoHomeCommand.Instance);
    }
}