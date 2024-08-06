namespace com.karabaev.applicationStateMachine
{
  public interface IStateFactory
  {
    TState Create<TState>();
  }
}