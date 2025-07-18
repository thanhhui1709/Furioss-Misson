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
                EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>(true);
                if (enemyHealth != null)
                {
                    group.DoDamage(enemyHealth);
                    CalculateRemainHit();
                
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
            OrientedRocket rocket = collision.gameObject.GetComponent<OrientedRocket>();
            if (rocket != null)
            {
                group.DoDamage(rocket);
                remainHit = 0;
                CalculateRemainHit();
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            EnemyHealth heath = collision.gameObject.GetComponentInChildren<EnemyHealth>(true);
            if (heath != null)
            {
                group.DoDamage(heath);
                remainHit = 0;
                CalculateRemainHit();
            }
        }
    }
    private void CalculateRemainHit()
    {
        remainHit--;
        if (remainHit <= 0)
        {
            transform.gameObject.SetActive(false);
        }
    }
}
