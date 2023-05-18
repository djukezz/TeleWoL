using Ninject.Modules;
using TeleWoL.States;

namespace LaserControl.IoC;

internal class StatesModule : NinjectModule
{
    public override void Load()
    {
        BindState<MainState>();
        BindState<AddTargetState>();
        BindState<DeleteTargetsState>();
        BindState<DeleteUsersState>();
        BindState<LoginState>();
        BindState<SettingsState>();
        BindState<AddUserState>();
        BindState<AddAdminState>();
    }

    private void BindState<T>() => Bind<T>().To<T>().InTransientScope();
}
