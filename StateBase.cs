namespace StateTest;

internal abstract class StateBase
{
    public abstract string Name { get; }
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

    protected readonly List<CommandBase> _commands = new List<CommandBase>();
}
