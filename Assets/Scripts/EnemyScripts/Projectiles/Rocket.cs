using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage;
    [SerializeField] private float explosionRadius;
    public GameObject explosionEffect;
    public AudioClip explosionSound;
    public AudioClip rocketSound;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private GameObject player;
    private Vector3 playerPos;


    [SerializeField] private bool isOriented;
    private float cooldownTime;
    private Vector3 originalPos;
    void Start()
    {
        cooldownTime = lifetime;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPos = player.transform.position;
        }
        originalPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldownTime -= Time.fixedDeltaTime;
        if (!isOriented)
        {
            Moving();
        }
        else
        {
            Moving(playerPos, originalPos);
        }




        if (cooldownTime <= 0)
        {
            Explosion();
            return;
        }
    }
    private void Explosion()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Assuming the enemy has a method to take damage
                collider.GetComponent<PlayerHealth>().TakeDamage(damage);
                audioSource.PlayOneShot(explosionSound);
            }
        }
        ObjectPoolManager.SpawnObject(explosionEffect, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);

        ObjectPoolManager.ReturnObject(gameObject);

    }
    private void Moving()
    {
        rb.linearVelocity = transform.up * speed;
        audioSource.PlayOneShot(rocketSound);
    }
    private void Moving(Vector3 targetPos, Vector3 startPos)
    {
        audioSource.PlayOneShot(rocketSound);
        float distanceTotal = Vector3.Distance(startPos, targetPos);
        float distanceTravelled = Vector3.Distance(transform.position, startPos);
        float remainingDistance = Vector3.Distance(transform.position, targetPos);

        Vector2 direction = (targetPos - transform.position).normalized;

        float currentSpeed = Mathf.Lerp(speed/3, speed, distanceTravelled / distanceTotal);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        rb.linearVelocity = direction * currentSpeed;

        // When close enough to previous playerPos, update destination
        if (remainingDistance < 0.1f) // you can tweak this threshold
        {
            originalPos = transform.position;
            playerPos = player.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Explosion();
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Explosion();
        }

    }
}
