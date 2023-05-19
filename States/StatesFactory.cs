using Ninject;

namespace TeleWoL.States;

internal sealed class StatesFactory
{
    private readonly IKernel _kernel;

    public StatesFactory(IKernel kernel)
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
