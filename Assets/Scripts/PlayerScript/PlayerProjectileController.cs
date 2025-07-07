using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    //[SerializeField] private int maxEnemyHit;
    private int remainHit;
    void Start()
    {
        //remainHit = maxEnemyHit;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
              
                enemy.GetEnemyHealth().TakeDamage(damage);
                //CalculateRemainHit(remainHit);
            }
        }else if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
            if(bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
        }
    }
    //private void CalculateRemainHit(int remain)
    //{
    //    remainHit--;
    //    if (remainHit <= 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
