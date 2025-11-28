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
      _stateFactoryType = typeof(TStateFactory);
      return this;
    }

    public StateMachineRegistrationBuilder AddState(Type type)
    {
      _stateTypes.Add(type);
      return this;
    }
    
    public StateMachineRegistrationBuilder AddState<TState>() where TState : IApplicationState
    {
      return AddState(typeof(TState));
    }
    
    public void Finish()
    {
      _scopeBuilder.Register<AppStateMachine>().AsSingleton();
      
      if (_stateFactoryType != null)
      {
        _scopeBuilder.Register(_stateFactoryType).As<IStateFactory>();
      }
      
      foreach (var stateType in _stateTypes)
      {
        _scopeBuilder.Register(stateType).AsTransient();
      }
    }
    
    internal StateMachineRegistrationBuilder(IScopeContainerBuilder scopeBuilder) => _scopeBuilder = scopeBuilder;
  }
}