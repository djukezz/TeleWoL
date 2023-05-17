namespace TeleWoL.Commands;

internal sealed class StringCommand : CommandBase
{
    public StringCommand()
    {
        Id = string.Empty;
    }

    public StringCommand(string text) => Text = text;
    public override bool Match(string cmd) => false;

    public static StringCommand? FromString(string cmd) =>
        string.IsNullOrWhiteSpace(cmd) ? null : new StringCommand(cmd);
}
