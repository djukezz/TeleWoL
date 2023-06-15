using TeleWoL.Settings;
using TeleWoL.WakeOnLan;

namespace TeleWoL.Commands;

internal class TurnOnCommand : CommandBase
{
    public TurnOnCommand(Target target, IWoLSender woLSender)
    {
        _target = target;
        _woLSender = woLSender;

        Id = $"turnOn_{target.Name}";
        Text = $"Wake up '{_target.Name}'";
    }

    private Target _target;
    private IWoLSender _woLSender;

    public Response Execute()
    {
        _woLSender.Wake(_target.Mac);
        return $"Turning on '{_target.Name}' {_target.Mac}";
    }
}
