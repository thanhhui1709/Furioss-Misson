using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    [SerializeField] private int maxEnemyHit;

    private int remainHit;
    void OnEnable()
    {
        remainHit = maxEnemyHit;

    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GroupProjectile group = GetComponentInParent<GroupProjectile>();
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {

                group.DoDamage(enemy.GetEnemyHealth());
                CalculateRemainHit(remainHit);
            }
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                group.DoDamage(bossHealth);
            }
            transform.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Rocket"))
        {
            Rocket rocket = collision.gameObject.GetComponent<Rocket>();
            if (rocket != null)
            {
                group.DoDamage(rocket);
                CalculateRemainHit(remainHit);
            }
        }
    }
    private void CalculateRemainHit(int remain)
    {
        remainHit--;
        if (remainHit <= 0)
        {
            transform.gameObject.SetActive(false);
        }
    }
}
