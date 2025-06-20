using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int healthAmount;
    [SerializeField] private float moveSpeed;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();

    }
    private void FindPlayer()
    {
        if (player == null) return;
        float playerRotation = player.transform.rotation.z;
        if (playerRotation == 0)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
        else if (playerRotation > 0 && playerRotation <= 90)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else if (playerRotation < 0 && playerRotation >= -90)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if (playerRotation == 180)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health= collision.GetComponent<PlayerHealth>();
            health.Health(healthAmount);
            Destroy(gameObject);
        }
    }
}
