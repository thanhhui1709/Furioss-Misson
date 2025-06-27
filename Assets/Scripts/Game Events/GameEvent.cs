using UnityEngine;
using UnityEngine.Events;

public class GameEvent : MonoBehaviour
{
    public UnityEvent onStageClear;
    public UnityEvent onGameOver;
    public UnityEvent onWinGame;
    public UnityEvent onPlayerDie;

    public void TriggerStageClearEvent()=> onStageClear?.Invoke();
    public void TriggerWinGameEvent()=> onWinGame?.Invoke();
    public void TriggerGameOverEvent()=> onGameOver?.Invoke();
    public void TriggerPlayerDieEvent()=>onPlayerDie?.Invoke();


}
