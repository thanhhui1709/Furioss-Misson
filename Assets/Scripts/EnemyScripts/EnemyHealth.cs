using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float displayTime = 1.5f;
    public DropItems items;

    private Camera mainCamera;
    private Coroutine hideBarCoroutine;

    void Awake()
    {
        currentHealth = maxHealth;
        mainCamera = Camera.main;


        UpdateHealthBar();
        healthBar.gameObject.SetActive(false);

    }

    void LateUpdate()
    {
        // Make health bar always face the camera
        if (healthBar != null && mainCamera != null)
        {
            healthBar.transform.rotation = mainCamera.transform.rotation;
            healthBar.transform.position = transform.root.position + offset;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;


        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true); // Show bar
            UpdateHealthBar();

            // Restart the timer if already showing
            if (hideBarCoroutine != null)
            {

                StopCoroutine(hideBarCoroutine);
            }

            hideBarCoroutine = StartCoroutine(HideHealthBarAfterTime());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.value = (float)currentHealth / maxHealth;
    }

    IEnumerator HideHealthBarAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        if (healthBar != null)
            healthBar.gameObject.SetActive(false);
    }

    private void Die()
    {
        Destroy(transform.root.gameObject);
        items.DropItem();
    }
}
