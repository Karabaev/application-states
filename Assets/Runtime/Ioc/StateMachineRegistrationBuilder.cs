using System;
using System.Collections.Generic;
using com.karabaev.applicationStateMachine.States;
using com.karabaev.ioc.abstractions;
using JetBrains.Annotations;

namespace com.karabaev.applicationStateMachine.Ioc
{
  [PublicAPI]
  public class StateMachineRegistrationBuilder
  {
    private readonly IScopeContainerBuilder _scopeBuilder;
    private readonly List<Type> _stateTypes = new();
    private Type? _stateFactoryType;

    public StateMachineRegistrationBuilder AddStateFactory<TStateFactory>() where TStateFactory : IStateFactory
    {
      return AddStateFactory(typeof(TStateFactory));
    }
    
    public StateMachineRegistrationBuilder AddStateFactory(Type type)
    {
      _stateFactoryType = type;
      return this;
    }

    public StateMachineRegistrationBuilder AddState<TState>() where TState : IApplicationState
    {
      return AddState(typeof(TState));
    }

    public StateMachineRegistrationBuilder AddState(Type type)
    {
      _stateTypes.Add(type);
      return this;
    }

    public void Finish()
    {
      _scopeBuilder.Register<AppStateMachine>().AsSingleton().Build();
      
      if (_stateFactoryType != null)
      {
        _scopeBuilder.Register(_stateFactoryType).As<IStateFactory>().Build();
      }
      
      foreach (var stateType in _stateTypes)
      {
        _scopeBuilder.Register(stateType).AsSelf().AsTransient().Build();
      }
    }
    
    internal StateMachineRegistrationBuilder(IScopeContainerBuilder scopeBuilder) => _scopeBuilder = scopeBuilder;
  }
}