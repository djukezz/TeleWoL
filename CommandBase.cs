namespace StateTest;

internal abstract class CommandBase
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public virtual bool Match(string cmd) => cmd == Id;
}
