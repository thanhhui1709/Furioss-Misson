using UnityEngine;

public class StraightProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed;
    [SerializeField] private float horizontalBound;
    [SerializeField] private float verticalBound;
    public int damage;

    public bool moveByLocalDirection;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (moveByLocalDirection)
        {
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(-1 * (transform.up * speed * Time.deltaTime), Space.World);
        }

        if(transform.position.y >= verticalBound ||
           transform.position.y < -verticalBound ||
           transform.position.x >= horizontalBound ||
           transform.position.x < -horizontalBound)
        {
            ObjectPoolManager.ReturnObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerHealth heath = collision.gameObject.GetComponent<PlayerHealth>();
            heath.TakeDamage(damage);

            ObjectPoolManager.ReturnObject(gameObject);
        }
    }
}
