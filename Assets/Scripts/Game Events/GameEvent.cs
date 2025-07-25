using UnityEngine;
using UnityEngine.Events;

public class GameEvent : MonoBehaviour
{
    public static GameEvent instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
            DontDestroyOnLoad(gameObject);

    }
    public UnityEvent onStageClear;
    public UnityEvent onGameOver;
    public UnityEvent onWinGame;
    public UnityEvent onPlayerDie;
    public UnityEvent onPlayerLevelUp;
    public UnityAction<int> onEnemyDie;
    public UnityEvent onStageStart;

    public void TriggerStageClearEvent() => onStageClear?.Invoke();
    public void TriggerWinGameEvent() => onWinGame?.Invoke();
    public void TriggerGameOverEvent() => onGameOver?.Invoke();
    public void TriggerPlayerDieEvent() => onPlayerDie?.Invoke();
    public void TriggerEnemyDieEvent(int exp) => onEnemyDie?.Invoke(exp);
    public void TriggerPlayerLevelUpEvent() => onPlayerLevelUp?.Invoke();
    public void TriggerStageStart() => onStageStart?.Invoke();


}
