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
    private AudioSource audioSource;

    private HashSet<int> processedAttackIDs = new HashSet<int>();

    void Awake()
    {
        GameEvent.instance.onPlayerLevelUp.AddListener(LevelUpHealth);
        currentHealth = maxHealth;
        mainCamera = Camera.main;


        UpdateHealthBar();
        healthBar.gameObject.SetActive(false);
        audioSource = GetComponentInParent<AudioSource>(true);


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

    public void TakeDamage(int damage, int attackID)
    {
        if(processedAttackIDs.Contains(attackID))
        {
            return; // Ignore if this attack ID has already been processed
        }
        currentHealth -= damage;
        processedAttackIDs.Add(attackID); 
        if(processedAttackIDs.Count > 100)
        {
            processedAttackIDs.Clear(); // Clear the set to prevent memory overflow
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
        GameEvent.instance.TriggerEnemyDieEvent(exp);   
        items.DropItem();
        audioSource.clip = deathSound;
        audioSource.Play();
        ObjectPoolManager.SpawnObject(deathEffect, transform.position, Quaternion.identity,ObjectPoolManager.PoolType.Particle);
        StartCoroutine(WaitThenDie());
       
    }
    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(transform.root.gameObject);
    }

    public void LevelUpHealth()
    {
        maxHealth += 10; // Increase max health by 10
        currentHealth = maxHealth; // Reset current health to max

    }
}
