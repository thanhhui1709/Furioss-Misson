using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed;
    public int damage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     transform.Translate(transform.up*speed*Time.deltaTime,Space.World);    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null) { 
              enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
