using System.Collections.Generic;
using System.Collections;
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
    public AudioClip shootSound;
    public AShootingController[] shootBehavior = new AShootingController[2];
    public GameObject projectilePrefab;
    public float damage = 40f;


    private TurretState state;
    private float cooldownTimer = 0f;
    private Transform player;
    private Rigidbody2D rb;

    private enum TurretState
    {
        Idle,
        InRange
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        state = TurretState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        RotateToPlayer(90f);
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
        shootBehavior[0].Shoot(this, transform, playerTransform, projectilePrefab);
        ObjectPoolManager.PlayAudio(shootSound, 1f);


    }
    private void ShootWhenPlayerInRange(Transform playerTransform)
    {
        StartCoroutine(PlayAudioMultipleTimes(5, shootSound));
        shootBehavior[1].Shoot(this, transform, playerTransform, projectilePrefab);
    }
    private void RotateToPlayer(float speed = 180f)
    {
        if (player == null) return;
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion angle = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, speed * Time.deltaTime);
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
    IEnumerator PlayAudioMultipleTimes(int times,AudioClip clip)
    {
        int i = 0;
        while (i < times)
        {
            ObjectPoolManager.PlayAudio(clip, 1f);
            yield return new WaitForSeconds(clip.length); 
            i++;
        }
        
    }
  
}
