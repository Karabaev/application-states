using System;
using System.Collections.Generic;
using com.karabaev.applicationStateMachine.Ioc;
using com.karabaev.applicationStateMachine.States.Contexts;
using com.karabaev.ioc.abstractions;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace com.karabaev.applicationStateMachine.States
{
  [PublicAPI]
  public abstract class StateMachineAppState<TContext> : ScopedAppState<TContext> where TContext : IScopedStateContext
  {
    protected AppStateMachine InternalStateMachine { get; private set; } = null!;
    
    public override UniTask EnterAsync(TContext context)
    {
      base.EnterAsync(context);
      InternalStateMachine = Resolve<AppStateMachine>();
      
      return UniTask.CompletedTask;
    }

    protected override void InstallScope(IScopeContainerBuilder scopeBuilder)
    {
      var registrationBuilder = scopeBuilder.RegisterStateMachineBuilder();
      
      foreach (var stateType in GetInternalAppStateTypes())
      {
        registrationBuilder.AddState(stateType);
      }
      
      registrationBuilder.Finish();
    }

    protected abstract IEnumerable<Type> GetInternalAppStateTypes(); 

    protected StateMachineAppState(AppStateMachine stateMachine) : base(stateMachine) { }
  }
}