using StateTest;
using TeleWoL.Settings;

namespace TeleWoL.Commands;

internal class TurnOnCommand : CommandBase
{
    public TurnOnCommand(Target target)
    {
        _target = target;
        Id = $"turnOn_{target.Name}";
        Text = $"Wake up '{_target.Name}'";
    }

    private Target _target;

    public Response Execute()
    {
        return $"Turning on '{_target.Name}' {_target.Mac}";
    }
}
