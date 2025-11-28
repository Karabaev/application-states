using Cysharp.Threading.Tasks;

namespace com.karabaev.applicationStateMachine.States
{
  public interface IApplicationState
  {
    UniTask ExitAsync();
  }
}