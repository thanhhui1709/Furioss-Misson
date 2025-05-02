using UnityEngine;


public class PowerUpController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float moveSpeed;

    void Start()
    {
        player=GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
    }
    private void FindPlayer()
    {
        float playerRotation = player.transform.rotation.z;
        if (playerRotation == 0)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }else if(playerRotation > 0 && playerRotation <= 90)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else if (playerRotation < 0 && playerRotation >= -90)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }else if (playerRotation == 180)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }
}
