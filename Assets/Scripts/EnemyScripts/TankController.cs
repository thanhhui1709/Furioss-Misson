using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class TankController : MonoBehaviour
{
    
    public float triggerRadius = 8f;
    public float shootCooldown = 3f;
    public GameObject projectilePrefab;
    public AudioClip shootSound;
    public float damage = 40f;
    private Transform weapon;
    public AShootingController shootBehavior;

    private float cooldownTimer = 0f;
    private Transform player;

    void Start()
    {
      
        weapon = transform.Find("Weapon");

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
