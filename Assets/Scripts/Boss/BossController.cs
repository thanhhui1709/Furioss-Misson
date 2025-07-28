
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
        public int maxHealth = 1000;
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
            foreach (var behavior in currentPhase.behaviors)
            {
                currentBehavior = behavior;

                yield return StartCoroutine(ExecuteSingeBehavior(behavior));
                if (behavior.shootPattern != null && behavior.projectile != null)
                {

                    yield return new WaitUntil(() => behavior.shootPattern.isFinished);
                }
                yield return new WaitForSeconds(Random.Range(behavior.minRestTime, behavior.maxRestTime));
            }
            yield return new WaitForSeconds(Random.Range(currentPhase.minSequenceBehaviorCooldown, currentPhase.maxSequenceBehaviorCooldown));


            // Wait before starting next behavior loop
        }
    }
    IEnumerator ExecuteSingeBehavior(BossBehavior behavior)
    {
        if (behavior.shootPattern != null && behavior.projectile != null)
        {
            AShootingController shootingController = Instantiate(behavior.shootPattern);
            behavior.shootPattern = shootingController;
        }
        if (behavior.movement != null)
        {
            behavior.movement = Instantiate(behavior.movement);
            behavior.movement.Initialize(transform);
            //shoot when movement is initialized
            if (behavior.delayBetweenMoveAndShot == 0 && behavior.shootPattern != null)
            {

                behavior.shootPattern.Shoot(this, transform, playerTransform, behavior.projectile);
                Debug.Log(behavior.shootPattern.isFinished + " Shooting pattern is finished: " + behavior.shootPattern.name);
            }
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
            if (behavior.projectile != null && !behavior.shootPattern.isFinished)
            {

                behavior.shootPattern.Shoot(this, transform, playerTransform, behavior.projectile);
            }
        }


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
        currentPhase = bossPhases[++currentPhaseIndex];
        StartCoroutine(ExecuteBehaviorLoop());
    }
    public int GetCurrentPhaseIndex()
    {
        return currentPhaseIndex;
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
                  
                    }

                }
                if (pattern is FollowPlayerPattern followPattern)
                {
                    PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                    if (playerHealth != null && !followPattern.hasDealDamage)
                    {
                        playerHealth.TakeDamage(followPattern.damage);
                        followPattern.hasDealDamage = true;
                       
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
