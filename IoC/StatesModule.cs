using Ninject;
using Ninject.Modules;
using TeleWoL.States;

namespace LaserControl.IoC;

internal class StatesModule : NinjectModule
{
    public override void Load()
    {
        //Bind<IKernel>().ToMethod(context => context.Kernel);

        BindState<MainState>();
        BindState<AddTargetState>();
        BindState<DeleteTargetsState>();
        BindState<DeleteUsersState>();
        BindState<LoginState>();
        BindState<SettingsState>();
        BindState<AddUserState>();
        BindState<AddAdminState>();

        Bind<StatesFactory>().ToSelf().InSingletonScope();
    }

    private void BindState<T>() => Bind<T>().ToSelf().InTransientScope();
}
