using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BossHealth : MonoBehaviour
{
   
    //health component
    [SerializeField]private float maxHealth;
    public Slider healthBar;
    public Image healthFill;
    public Gradient gradient;
    public ParticleSystem explosion;
    public AudioSource explosionSound;


    private float currentHealth;

    public void Initialize(Slider externalHealthBar,Image externalHealthFill)
    {
        healthBar = externalHealthBar;
        healthFill= externalHealthFill;
        healthBar.gameObject.SetActive(true);
        currentHealth = maxHealth;
        UpdateHealthBar();
      
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
        GameEvent.instance.TriggerStageClearEvent();
        explosion.Play();
        explosionSound.Play();
        healthFill.enabled = false;
        StartCoroutine(DieAfterDelay(1f));

    }
    private void UpdateHealthBar()
    {
        float value =(float)currentHealth/maxHealth;
        healthBar.value=value;
        healthFill.color = gradient.Evaluate(value);
    }
    private IEnumerator DieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(transform.root.gameObject);
    }

}
