using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal class DeleteTargetState : UserStateBase
{
    public DeleteTargetState(UserSettings settings) : base(settings)
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
        if (command is GoHomeCommand)
        {
            newState = new MainState(_settings);
            return null;
        }

        return Response.Unknown;
    }

    private void Update()
    {
        _commands.Clear();
        _commands.AddRange(_settings.GetTargets()
            .Select(t => new DeleteTargetCommand(t, _settings)));
        _commands.Add(GoHomeCommand.Instance);
    }
}
