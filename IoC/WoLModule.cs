using Ninject;
using Ninject.Modules;
using TeleWoL.Settings;
using TeleWoL.WakeOnLan;

namespace TeleWoL.IoC;

internal class WoLModule : NinjectModule
{
    public override void Load()
    {
        var settings = Kernel.Get<GlobalSettings>();
        var sender = new MultipleNicWoLSender(); // NICs can be selected here
        Bind<IWoLSender>().ToMethod(_ => sender).InSingletonScope();
    }
}
