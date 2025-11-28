using com.karabaev.applicationStateMachine.States.Contexts;
using com.karabaev.ioc.abstractions;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace com.karabaev.applicationStateMachine.States
{
  [PublicAPI]
  public abstract class ScopedAppState<TContext> : AppState<TContext> where TContext : IScopedStateContext
  {
    protected IScope Scope { get; private set; } = null!;

    public override UniTask EnterAsync(TContext context)
    {
      Scope = context.ParentScope.CreateChild(InstallScope);
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      Scope.Dispose();
      return UniTask.CompletedTask;
    }
    
    protected abstract void InstallScope(IScopeContainerBuilder builder);

    protected T Resolve<T>() => Scope.ObjectResolver.Resolve<T>();

    protected ScopedAppState(AppStateMachine stateMachine) : base(stateMachine) { }
  }
}