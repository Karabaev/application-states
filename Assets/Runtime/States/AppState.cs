using com.karabaev.applicationStateMachine.States.Contexts;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace com.karabaev.applicationStateMachine.States
{
  [PublicAPI]
  public abstract class AppState<TContext> : IApplicationState
  {
    private readonly AppStateMachine _stateMachine;
    
    public abstract UniTask EnterAsync(TContext context);

    public virtual UniTask ExitAsync() => UniTask.CompletedTask;

    protected UniTask EnterNextStateAsync<TState, TNextContext>(TNextContext context) where TState : AppState<TNextContext>
      => _stateMachine.EnterAsync<TState, TNextContext>(context);

    protected UniTask EnterNextStateAsync<TState>() where TState : AppState<EmptyStateContext>
      => _stateMachine.EnterAsync<TState>();
    
    protected AppState(AppStateMachine stateMachine) => _stateMachine = stateMachine;
  }
}