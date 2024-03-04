namespace com.karabaev.applicationLifeCycle.StateMachine
{
  public interface IStateFactory
  {
    TState Create<TState>();
  }
}