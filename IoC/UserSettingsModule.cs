using Ninject;
using Ninject.Modules;
using StateTest;
using TeleWoL.Settings;

namespace TeleWoL.IoC;

internal class UserSettingsModule : NinjectModule
{
    private readonly UserContext _userContext;

    public UserSettingsModule(UserContext userContext)
    {
        _userContext = userContext;
    }

    public override void Load()
    {
        Bind<UserContext>().ToConstant(_userContext).InSingletonScope();
        Bind<UserSettings>().ToMethod(context => context.Kernel.Get<GlobalSettings>().GetOrAdd(_userContext.UserId)).InSingletonScope();
    }
}
