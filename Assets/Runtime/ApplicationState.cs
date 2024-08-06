using Cysharp.Threading.Tasks;

namespace com.karabaev.applicationStateMachine
{
  public interface IApplicationState
  {
    UniTask ExitAsync();
  }
  
  public abstract class ApplicationState<TContext> : IApplicationState
  {
    private readonly ApplicationStateMachine _stateMachine;

    public abstract UniTask EnterAsync(TContext context);

    public abstract UniTask ExitAsync();

    protected UniTask EnterNextStateAsync<TState, TNextContext>(TNextContext context) where TState : ApplicationState<TNextContext>
      => _stateMachine.EnterAsync<TState, TNextContext>(context);

    protected UniTask EnterNextStateAsync<TState>() where TState : ApplicationState<EmptyStateContext>
      => _stateMachine.EnterAsync<TState>();
    
    protected ApplicationState(ApplicationStateMachine stateMachine) => _stateMachine = stateMachine;
  }
}