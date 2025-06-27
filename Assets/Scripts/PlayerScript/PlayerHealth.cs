using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameEvent gameEvent;

    [SerializeField] private float maxHealth;
    
    public Slider healthBar;
    public Image healthFill;
    public Gradient gradient;
    public bool hasShield;
    
    private bool isDead = false;
    private float currentHealth;

    //
    [Header("Respawn Stats")]
    public GameObject shieldIndicator;
    [SerializeField] private float shieldDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
       
        
 
    }


    public void TakeDamage(int damage)
    {
        if (hasShield || isDead || currentHealth <= 0) return;

        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();

        }
    }
    public void Die()
    {
        if (isDead) return; 
        isDead = true;
        GameManager.instance.numberOfLive--;
        healthFill.enabled = false;
 
        if (GameManager.instance.numberOfLive<=0)
        {

            gameEvent.TriggerGameOverEvent();
          
        }
        else
        {
            gameEvent.TriggerPlayerDieEvent();
        }
        transform.gameObject.SetActive(false);

    }
    private void UpdateHealthBar()
    {
        float value = currentHealth / maxHealth;
        healthBar.value = value;
        healthFill.color = gradient.Evaluate(value);
    }
    public void Health(int amount)
    {

        float newHealth = currentHealth + amount;
        if (newHealth >= maxHealth)
        {

            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = newHealth;

        }
        UpdateHealthBar();
    }

    IEnumerator DisableShieldEffect(float duration, GameObject indicator)
    {
        yield return new WaitForSeconds(duration);
        hasShield = false;
        indicator.gameObject.SetActive(false);
    }
    IEnumerator DisableShieldEffect()
    {
        yield return new WaitForSeconds(shieldDuration);
        hasShield = false;
        shieldIndicator.gameObject.SetActive(false);
    }
    public void ApplyShieldEffect(GameObject indicator, float duration)
    {
        hasShield = true;
        indicator.gameObject.SetActive(true);
        StartCoroutine(DisableShieldEffect(duration, indicator));
    } 
    public void ApplyShieldEffect()
    {
        hasShield = true;
        shieldIndicator.gameObject.SetActive(true);
        StartCoroutine(DisableShieldEffect());
    }
    private void OnEnable()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        healthFill.enabled = true;
        isDead=false;
      

    }

}
