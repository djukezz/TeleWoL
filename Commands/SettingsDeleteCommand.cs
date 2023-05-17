namespace TeleWoL.Commands;

internal class SettingsDeleteCommand : CommandBase
{
    private SettingsDeleteCommand()
    {
        Text = "> Delete target";
    }

    public static readonly SettingsDeleteCommand Instance = new SettingsDeleteCommand();
}
