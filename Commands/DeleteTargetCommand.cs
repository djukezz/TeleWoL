using TeleWoL.Settings;

namespace TeleWoL.Commands;

internal class DeleteTargetCommand : CommandBase
{
    public DeleteTargetCommand(Target target, UserSettings settings)
    {
        _target = target;
        _settings = settings;
        Id = $"delete_{target.Name}";
        Text = _target.Name;
    }

    private Target _target;
    private readonly UserSettings _settings;

    public Response Execute()
    {
        bool res = _settings.DeleteTarget(_target);
        return $"Target '{_target.Name} ' deleted";
    }
}
