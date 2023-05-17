namespace TeleWoL.Commands;

internal abstract class CommandBase
{
    protected CommandBase()
    {
        Id = GetType().Name;
    }

    public string Id { get; set; }
    public string Text { get; init; } = string.Empty;
    public virtual bool Match(string cmd) => cmd == Id;
}
