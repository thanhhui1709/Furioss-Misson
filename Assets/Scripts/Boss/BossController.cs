
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [System.Serializable]
    public class BossBehavior
    {
        public IMovementPattern movement;
        public AShootingController shootPattern;
        public GameObject projectile;
        public float delayBetweenMoveAndShot;
        public float minRestTime;
        public float maxRestTime;
    }

    public List<BossBehavior> behaviours;
    [SerializeField] private float minSequenceBehaviorCooldown;
    [SerializeField] private float maxSequenceBehaviorCooldown;
    [SerializeField] private float activeTime = 4f;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnSpeed;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector3 startPos;
    private bool isDoneSpawned;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(ExecuteBehaviorLoop());
        isDoneSpawned = false;

    }

    private IEnumerator ExecuteBehaviorLoop()
    {
        yield return new WaitUntil(() => !Spawn());
        yield return new WaitForSeconds(activeTime);
        while (true)
        {
           
            
             var behavior = behaviours[Random.Range(0,behaviours.Count)];

             yield return   StartCoroutine(ExecuteSingeBehavior(behavior));


            yield return new WaitForSeconds(Random.Range(minSequenceBehaviorCooldown, maxSequenceBehaviorCooldown));
            
           
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
        if (behavior.shootPattern != null&& behavior.projectile!=null)
        {
            behavior.shootPattern.Shoot(this,transform, playerTransform, behavior.projectile);
        }
        yield return new WaitForSeconds(Random.Range(behavior.minRestTime, behavior.maxRestTime));

    }
    void Update()
    {
        if (!isDoneSpawned) { Spawn(); };

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
            isDoneSpawned = true;
            return false;
         
        }
        

    }



}
