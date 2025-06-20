
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [System.Serializable]
    public class BossBehavior
    {
        public IMovementPattern movement;
        public AShootingController shootPattern;
        public float delayBetweenMoveAndShot;
        public float restTime;
    }

    public List<BossBehavior> behaviours;
    public GameObject bossProjectiles;
    [SerializeField] private float sequenceBehaviorCooldown = 2f;
    [SerializeField] private float activeTime = 4f;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnSpeed;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector3 startPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(ExecuteBehaviorLoop());

    }

    private IEnumerator ExecuteBehaviorLoop()
    {
        yield return new WaitUntil(() => !Spawn());
        yield return new WaitForSeconds(activeTime);
        while (true)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                var behavior = behaviours[i];

             yield return   StartCoroutine(ExecuteSingeBehavior(behavior));


            yield return new WaitForSeconds(sequenceBehaviorCooldown);
            }
           
            // Wait before starting next behavior loop
        }
    }
    IEnumerator ExecuteSingeBehavior(BossBehavior behavior)
    {
        if (behavior.movement != null)
        {
            behavior.movement.Initialize(transform);

            while (!behavior.movement.isFinished)
            {
                behavior.movement.UpdateMovement(transform, Time.deltaTime);
                yield return null;
            }

            // Wait before shooting
            if (behavior.delayBetweenMoveAndShot > 0f)
            {
                yield return new WaitForSeconds(behavior.delayBetweenMoveAndShot);
            }
        }



        // Shoot
        if (behavior.shootPattern != null)
        {
            behavior.shootPattern.Shoot(transform, playerTransform, bossProjectiles);
        }
        yield return new WaitForSeconds(behavior.restTime);

    }
    void Update()
    {
        Spawn();

    }
    private bool Spawn()
    {

        float distanceTravelled = Vector3.Distance(transform.position, startPos);
        if (distanceTravelled < spawnDistance)
        {
            float remain = Mathf.Clamp01(spawnDistance - (distanceTravelled + 0.1f) / spawnDistance);
            float currentSpeed=spawnSpeed*remain;
            rb.linearVelocity = Vector2.down * currentSpeed;
            return true;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        return false;

    }



}
