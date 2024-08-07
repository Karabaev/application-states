using System;
using Cysharp.Threading.Tasks;

namespace com.karabaev.applicationStateMachine
{
  public class ApplicationStateMachine : IDisposable
  {
    private readonly IStateFactory _stateFactory;
    
    private IApplicationState? _activeState;

    public async UniTask EnterAsync<TState, TContext>(TContext context) where TState : ApplicationState<TContext>
    {
      if(_activeState != null)
        await _activeState.ExitAsync();

      var state = _stateFactory.Create<TState>();
      _activeState = state;
      await state.EnterAsync(context);
    }

    public UniTask EnterAsync<TState>() where TState : ApplicationState<EmptyStateContext>
    {
      return EnterAsync<TState, EmptyStateContext>(default);
    }

    public void Dispose() => _activeState?.ExitAsync().Forget();

    public ApplicationStateMachine(IStateFactory stateFactory) => _stateFactory = stateFactory;
  }
}