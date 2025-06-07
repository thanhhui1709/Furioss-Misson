using UnityEngine.UI;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    [SerializeField]private Slider healthBar;
    [SerializeField] private Vector3 offset;
    
        
    void Awake()
    {
        currentHealth=maxHealth;
        healthBar.transform.position=transform.position+offset;
        UpdateHealthBar();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
       

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth/maxHealth;
    }
}
