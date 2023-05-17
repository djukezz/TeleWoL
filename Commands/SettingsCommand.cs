namespace TeleWoL.Commands;

internal class SettingsCommand : CommandBase
{
    private SettingsCommand()
    {
        Text = "> Settings";
    }

    public static readonly SettingsCommand Instance = new SettingsCommand();
}
