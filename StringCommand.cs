namespace StateTest;

internal sealed class StringCommand : CommandBase
{
    public StringCommand(string text) => Text = text;
    public override bool Match(string cmd) => false;
}
