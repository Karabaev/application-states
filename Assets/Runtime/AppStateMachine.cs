using System;
using com.karabaev.applicationStateMachine.States;
using com.karabaev.applicationStateMachine.States.Contexts;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace com.karabaev.applicationStateMachine
{
  [PublicAPI]
  public class AppStateMachine : IDisposable
  {
    private readonly IStateFactory _stateFactory;
    
    private IApplicationState? _activeState;

    public async UniTask EnterAsync<TState, TContext>(TContext context) where TState : AppState<TContext>
    {
      if(_activeState != null)
        await _activeState.ExitAsync();

      var state = _stateFactory.Create<TState>();
      _activeState = state;
      await state.EnterAsync(context);
    }

    public UniTask EnterAsync<TState>() where TState : AppState<EmptyStateContext>
    {
      return EnterAsync<TState, EmptyStateContext>(default);
    }

    public void Dispose() => _activeState?.ExitAsync().Forget();

    public AppStateMachine(IStateFactory stateFactory) => _stateFactory = stateFactory;
  }
}