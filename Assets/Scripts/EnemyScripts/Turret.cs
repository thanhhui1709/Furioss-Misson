using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Turret : MonoBehaviour
{
   
    [Header("InRange State")]
    public float triggerRadius = 8f;
    public float shootCooldown = 3f;

    [Header("Idle State")]
    public float shootIdleCooldownMin = 1f;
    public float shootIdleCooldownMax = 3f;
    [Header("Common")]
    public AShootingController[] shootBehavior = new AShootingController[2];
    public GameObject projectilePrefab;
    public float damage = 40f;


    private TurretState state;
    private float cooldownTimer = 0f;
    private Transform player;

    private enum TurretState
    {
        Idle,
        InRange
    }
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        state = TurretState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        cooldownTimer -= Time.deltaTime;
        if (CheckPlayerInRange())
        {
            state = TurretState.InRange;
            Debug.Log("Player in range: " + player.name);
        }
        else
        {
            state = TurretState.Idle;
        }
        switch (state)
        {
            case TurretState.Idle:
                if (cooldownTimer <= 0f)
                {
                    ShootIdle(player);
                    cooldownTimer = Random.Range(shootIdleCooldownMin, shootIdleCooldownMax);
                }

                break;
            case TurretState.InRange:
                if (cooldownTimer <= 0f)
                {
                    ShootWhenPlayerInRange(player);
                    cooldownTimer = shootCooldown;
                }
                break;
        }
    }
    private void ShootIdle(Transform playerTransform)
    {
        if (shootBehavior.Length == 0) return;
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        shootBehavior[0].Shoot(this, transform, playerTransform, projectilePrefab);


    }
    private void ShootWhenPlayerInRange(Transform playerTransform)
    {
        shootBehavior[1].Shoot(this, transform, playerTransform, projectilePrefab);
    }
    private bool CheckPlayerInRange()
    {
        if (player == null) return false;
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= triggerRadius;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
  
}
