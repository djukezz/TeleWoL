using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal class DeleteTargetsState : StateBase
{
    public override void Init()
    {
        Update();
    }

    protected override Response? Execute(CommandBase command, out StateBase newState)
    {
        newState = this;
        if (command is DeleteTargetCommand delete)
        {
            var res = delete.Execute();
            Update();
            return res;
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
        _commands.AddRange(UserSettings.GetTargets()
            .Select(t => new DeleteTargetCommand(t, UserSettings)));
        _commands.Add(GoHomeCommand.Instance);
    }
}
