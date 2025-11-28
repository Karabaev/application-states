using com.karabaev.ioc.abstractions;

namespace com.karabaev.applicationStateMachine.States.Contexts
{
  public interface IScopedStateContext
  {
    IScope ParentScope { get; }
  }
}