
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
    [System.Serializable]
    public class Phase
    {
        public List<BossBehavior> behaviors;
        public float minSequenceBehaviorCooldown;
        public float maxSequenceBehaviorCooldown;
        public float activeTime = 4f;
    }  
    public List<Phase> bossPhases;
    public float timeBetweenPhases = 5f;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnSpeed;

    private Phase currentPhase;
    private int currentPhaseIndex;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector3 startPos;
    private bool isDoneSpawned;
    private BossBehavior currentBehavior;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPhaseIndex = 0;
        currentPhase = bossPhases[currentPhaseIndex];
        startPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(ExecuteBehaviorLoop());
        isDoneSpawned = false;

    }

    private IEnumerator ExecuteBehaviorLoop()
    {
        yield return new WaitUntil(() => !Spawn());
        yield return new WaitForSeconds(currentPhase.activeTime);
        while (true)
        {
            foreach(var behavior in currentPhase.behaviors)
            {
                currentBehavior = behavior;

                yield return StartCoroutine(ExecuteSingeBehavior(behavior));
            }
            yield return new WaitForSeconds(Random.Range( currentPhase. minSequenceBehaviorCooldown, currentPhase.maxSequenceBehaviorCooldown));


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
        if (behavior.shootPattern != null && behavior.projectile != null)
        {
            behavior.shootPattern.Shoot(this, transform, playerTransform, behavior.projectile);
        }
        yield return new WaitForSeconds(Random.Range(behavior.minRestTime, behavior.maxRestTime));

    }
    void Update()
    {
        if (!isDoneSpawned) { Spawn(); } 

    }
    private bool Spawn()
    {

        float distanceTravelled = Vector3.Distance(transform.position, startPos);
        if (distanceTravelled < spawnDistance)
        {
            float remain = Mathf.Clamp01(spawnDistance - (distanceTravelled + 0.1f) / spawnDistance);
            float currentSpeed = spawnSpeed * remain;
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
    public bool checkFinalPhase()
    {
        return bossPhases.IndexOf(currentPhase) == bossPhases.Count - 1;
    }
    public void ReSpawn()
    {
        isDoneSpawned = false;
        transform.position = startPos;
        currentPhase = bossPhases[++currentPhaseIndex];
        StartCoroutine(ExecuteBehaviorLoop());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentBehavior != null)
            {
                var pattern = currentBehavior.movement;
                // check pattern is maphite pattern
                if (pattern is MaphitePattern maphitePattern)
                {
                    PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                    if (playerHealth != null && !maphitePattern.hasDealDamage)
                    {
                        playerHealth.TakeDamage(maphitePattern.damage);
                        maphitePattern.hasDealDamage = true;
                        Debug.Log("Boss collided with player and dealt damage: " + maphitePattern.damage);
                    }

                }

            }
            else
            {
                return;
            }


        }



    }
}
