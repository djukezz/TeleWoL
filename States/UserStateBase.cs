using TeleWoL.Settings;

namespace TeleWoL.States;

internal abstract class UserStateBase : StateBase
{
    protected readonly UserSettings _settings;

    protected UserStateBase(UserSettings settings)
    {
        _settings = settings;
    }
}
