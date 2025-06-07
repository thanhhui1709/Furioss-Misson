using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private int maxEnemyHit;
    private int remainHit;
    void Start()
    {
        remainHit = maxEnemyHit;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HealthController health = collision.gameObject.GetComponent<HealthController>();
            if (health != null)
            {
                health.TakeDamage(damage);
                CalculateRemainHit(remainHit);
            }
        }
    }
    private void CalculateRemainHit(int remain)
    {
        remainHit--;
        if (remainHit <= 0)
        {
            Destroy(gameObject);
        }
    }
}
