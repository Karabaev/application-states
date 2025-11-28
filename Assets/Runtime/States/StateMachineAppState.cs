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
    
    public override async UniTask EnterAsync(TContext context)
    {
      await base.EnterAsync(context);
      InternalStateMachine = Resolve<AppStateMachine>();
    }

    protected override void InstallScope(IScopeContainerBuilder scopeBuilder)
    {
      var registrationBuilder = scopeBuilder.RegisterStateMachineBuilder();
      registrationBuilder.AddStateFactory(InternalAppStateFactoryType);
      
      foreach (var stateType in InternalAppStateTypes)
      {
        registrationBuilder.AddState(stateType);
      }
      
      registrationBuilder.Finish();
    }

    protected abstract IEnumerable<Type> InternalAppStateTypes { get; }

    protected abstract Type InternalAppStateFactoryType { get; }
    
    protected StateMachineAppState(AppStateMachine stateMachine) : base(stateMachine) { }
  }
}