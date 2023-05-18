using Ninject;
using StateTest;
using TeleWoL.Commands;
using TeleWoL.Settings;

namespace TeleWoL.States;

internal abstract class StateBase
{
    public virtual void Init() { }
    public virtual string Description { get; } = string.Empty;
    public IReadOnlyCollection<CommandBase> Commands => _commands;
    protected abstract Response? Execute(CommandBase command, out StateBase newState);
    protected virtual CommandBase? ConvertCommand(string command) => null;
    public virtual Response? Execute(string command, out StateBase newState)
    {
        var cmd = Commands.FirstOrDefault(c => c.Match(command));
        if (cmd == null)
            cmd = ConvertCommand(command);
        if (cmd != null)
            return Execute(cmd, out newState);
        newState = this;
        return Response.Unknown;
    }
    public event Action? SettingsChanged;

    protected void FireSettingsChanged() => SettingsChanged?.Invoke();
    protected readonly List<CommandBase> _commands = new List<CommandBase>();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Inject]
    public UserContext UserContext { get; set; }
    [Inject]
    public GlobalSettings GlobalSettings { get; set; }
    [Inject]
    public UserSettings UserSettings { get; set; }
    [Inject]
    public StatesFactory StatesFactory { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
