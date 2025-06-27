using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BossHealth : MonoBehaviour
{
    //Event
    private GameEvent gameEvent;
    //health component
    [SerializeField]private float maxHealth;
    private Slider healthBar;
    private Image healthFill;
    public Gradient gradient;


    private float currentHealth;

    public void Initialize(Slider externalHealthBar,Image externalHealthFill,GameEvent gameEvent)
    {
        healthBar = externalHealthBar;
        healthFill= externalHealthFill;
        healthBar.gameObject.SetActive(true);
        currentHealth = maxHealth;
        UpdateHealthBar();
        this.gameEvent = gameEvent;
    }

    public void TakeDamage(int damage)
    {
        currentHealth-=damage;
        UpdateHealthBar() ;
        if (currentHealth <= 0) {
            Die();
        }

    }
    private void Die()
    {
        gameEvent.TriggerStageClearEvent();
        Destroy(transform.root.gameObject);
        healthFill.enabled = false;
    }
    private void UpdateHealthBar()
    {
        float value =(float)currentHealth/maxHealth;
        healthBar.value=value;
        healthFill.color = gradient.Evaluate(value);
    }
}
