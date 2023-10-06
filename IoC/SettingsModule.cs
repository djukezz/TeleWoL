using Ninject;
using Ninject.Modules;
using TeleWoL.Settings;

namespace TeleWoL.IoC;

internal class SettingsModule : NinjectModule
{
    private string _settingsFilePath;

    public SettingsModule(string settingsFilePath)
    {
        _settingsFilePath = settingsFilePath;
    }

    public override void Load()
    {
        Bind<SettingsWrapper<GlobalSettings>, ISettingsSaver>()
            .ToConstant(new SettingsWrapper<GlobalSettings>(_settingsFilePath))
            .InSingletonScope();
        Bind<GlobalSettings>().ToMethod(context => context.Kernel.Get<SettingsWrapper<GlobalSettings>>().Settings).InSingletonScope();
    }
}
