using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public Slider healthBar;
    public Image healthFill;
    public Gradient gradient;
    public bool hasShield;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        if (hasShield) return;
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth < damage)
        {
            Die();

        }
    }
    public void Die()
    {
        Destroy(gameObject);
        healthFill.enabled = false;
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

    IEnumerator DisableShieldEffect(float duration,GameObject indicator)
    {
        yield return new WaitForSeconds(duration);
        hasShield = false;
        indicator.gameObject.SetActive(false);
    }
    public void ApplyShieldEffect(GameObject indicator,float duration)
    {
        hasShield = true;
        indicator.gameObject.SetActive(true);
        StartCoroutine(DisableShieldEffect(duration,indicator));
    }

}
