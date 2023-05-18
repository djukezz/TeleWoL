using TeleWoL.States;

namespace TeleWoL.Commands;

internal class StateCommandBase<T> : CommandBase, IStateCommand
    where T : StateBase
{
    public T Execute(StatesFactory statesFactory) => statesFactory.Create<T>();
    StateBase IStateCommand.Execute(StatesFactory statesFactory) => Execute(statesFactory);
}

internal interface IStateCommand
{
    StateBase Execute(StatesFactory statesFactory);
}
