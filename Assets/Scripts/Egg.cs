using UnityEngine;

public class Egg : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject brokenEgg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Sensor"))
        {
            Instantiate(brokenEgg,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
