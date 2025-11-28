using com.karabaev.ioc.abstractions;

namespace com.karabaev.applicationStateMachine.Ioc
{
  public static class IocExtensions
  {
    public static StateMachineRegistrationBuilder RegisterStateMachineBuilder(this IScopeContainerBuilder scopeBuilder)
    {
      return new StateMachineRegistrationBuilder(scopeBuilder);
    }
  }
}