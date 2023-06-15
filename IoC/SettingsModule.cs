using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using TeleWoL.Settings;

namespace TeleWoL.IoC;

internal class SettingsModule : NinjectModule
{
    public override void Load()
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
        dir = Path.Combine(dir, "TeleWoL");
        if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        const string settingsFile = "settings.json";
        string filePath = Path.Combine(dir, settingsFile);

        Bind<SettingsWrapper<GlobalSettings>, ISettingsSaver>()
            .ToConstant(new SettingsWrapper<GlobalSettings>(filePath))
            .InSingletonScope();
        Bind<GlobalSettings>().ToMethod(context => context.Kernel.Get<SettingsWrapper<GlobalSettings>>().Settings).InSingletonScope();
    }
}
