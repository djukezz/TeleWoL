using TeleWoL.States;

namespace TeleWoL.Commands;

internal class SettingsCommand : StateCommandBase<SettingsState>
{
    private SettingsCommand()
    {
        Text = "> Settings";
    }

    public static readonly SettingsCommand Instance = new SettingsCommand();
}
