using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using TeleWoL.Settings;

namespace TeleWoL.IoC;

internal class SettingsModule : NinjectModule
{
    public override void Load()
    {
        const string settingsFile = "settings.json";
        Bind<SettingsWrapper<GlobalSettings>, ISettingsSaver>()
            .ToConstant(new SettingsWrapper<GlobalSettings>(settingsFile))
            .InSingletonScope();
        Bind<GlobalSettings>().ToMethod(context => context.Kernel.Get<SettingsWrapper<GlobalSettings>>().Settings).InSingletonScope();
    }
}
