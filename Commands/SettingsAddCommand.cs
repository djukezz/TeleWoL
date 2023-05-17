namespace TeleWoL.Commands;

internal class SettingsAddCommand : CommandBase
{
    private SettingsAddCommand()
    {
        Text = "> Add target";
    }

    public static readonly SettingsAddCommand Instance = new SettingsAddCommand();
}