using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float displayTime = 1.5f;
    [SerializeField] private int exp;
    public DropItems items;
    public GameObject deathEffect;
    public AudioClip deathSound;

    private Camera mainCamera;
    private Coroutine hideBarCoroutine;
    private Queue<int> processedAttackIDs = new Queue<int>();
    private const int MaxAttackIDs = 100;
    private bool hasDied;

    void Awake()
    {
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        hasDied = false;
        currentHealth = maxHealth; // Reset health when enemy is enabled
        UpdateHealthBar();
        if (healthBar != null)
            healthBar.gameObject.SetActive(false); // Hide health bar initially
        processedAttackIDs.Clear(); // Clear processed attack IDs when enemy is enabled
    }

    private void OnDisable()
    {
        if (hideBarCoroutine != null)
        {
            StopCoroutine(hideBarCoroutine);
            hideBarCoroutine = null;
        }
    }

    void LateUpdate()
    {
        // Make health bar always face the camera
        if (healthBar != null && mainCamera != null)
        {
            healthBar.transform.rotation = mainCamera.transform.rotation;
            healthBar.transform.position = transform.position + offset;
        }
    }

    public void TakeDamage(float damage, int attackID)
    {
        if (hasDied) return; // Ignore if already dead
        if (processedAttackIDs.Contains(attackID))
        {
            return; // Ignore if this attack ID has already been processed
        }
        currentHealth -= damage;
        processedAttackIDs.Enqueue(attackID);
        if (processedAttackIDs.Count > MaxAttackIDs)
        {
            processedAttackIDs.Dequeue(); // Remove oldest
        }

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
        if (healthBar != null)
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
        hasDied=true;
        GameEvent.instance.TriggerEnemyDieEvent(exp);
        items.DropItem();
        ObjectPoolManager.PlayAudio(deathSound, 1f);
        ObjectPoolManager.SpawnObject(deathEffect, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);
        if (gameObject.transform.parent != null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            ObjectPoolManager.ReturnObject(gameObject);
        }
       
    }
}
