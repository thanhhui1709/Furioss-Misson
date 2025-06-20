using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
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
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerHealth heath = collision.gameObject.GetComponent<PlayerHealth>();
            heath.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
