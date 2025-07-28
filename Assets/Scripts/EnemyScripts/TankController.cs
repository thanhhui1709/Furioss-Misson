using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;


public class TankController : MonoBehaviour
{
    
    public float triggerRadius = 8f;
    public float shootCooldown = 3f;
    public GameObject projectilePrefab;
    public AudioClip shootSound;
    public float damage = 40f;
    public int bodyDamage = 50;
    public float bodyDamageCooldown = 1f; 
    private Transform weapon;
    public AShootingController shootBehavior;
    private bool hasCollidedWithPlayer = false;
    private float cooldownTimer = 0f;
    private Transform player;
    private float bodyDamageTimer = 0f;


    void Start()
    {
      
        weapon = transform.Find("Weapon");
        bodyDamageTimer=bodyDamageCooldown;
        // Find player in scene by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Calculate distance to player
        float distance = Vector2.Distance(transform.position, player.position);

        // If player is within triggerRadius and not cooling down, shoot
        if (distance <= triggerRadius && cooldownTimer <= 0f)
        {
            StartCoroutine(ShootAtPlayer(player));
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
        if(hasCollidedWithPlayer)
        {
            bodyDamageTimer -= Time.deltaTime;
            if (bodyDamageTimer <= 0f)
            {
                hasCollidedWithPlayer = false;
                bodyDamageTimer = 1f; 
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollidedWithPlayer)
        {
            hasCollidedWithPlayer = true;
            PlayerHealth playerController = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerController != null)
            {
                playerController.TakeDamage(bodyDamage);
            }
        }
    }

    IEnumerator ShootAtPlayer(Transform playerTransform)
    {
        shootBehavior.Shoot(this, weapon, playerTransform, projectilePrefab);
        ObjectPoolManager.PlayAudio(shootSound, 1f);
        cooldownTimer = shootCooldown;
        yield return null;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }

}
