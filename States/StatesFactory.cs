using Ninject;

namespace TeleWoL.States;

internal sealed class StatesFactory
{
    private readonly StandardKernel _kernel;

    public StatesFactory(StandardKernel kernel)
    {
        _kernel = kernel;
    }

    public T Create<T>()
        where T : StateBase
    {
        var state = _kernel.Get<T>();
        state.Init();
        return state;
    }
}
