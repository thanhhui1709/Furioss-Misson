using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 5f;
    [SerializeField] private float verticalBound = 3.8f;
    [SerializeField] private float horizontalBound = 7.5f;
   
    void Start()
    {
   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        MovePlayer();
    

    }
    private void MovePlayer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        direction.z = 0;

        if (transform.position.x >= horizontalBound)
        {
            transform.position = new Vector3(horizontalBound, transform.position.y, 0);
        }
        else if (transform.position.x <= -horizontalBound)
        {
            transform.position = new Vector3(-horizontalBound, transform.position.y, 0);
        }
        if (transform.position.y >= verticalBound)
        {
            transform.position = new Vector3(transform.position.x, verticalBound, 0);
        }
        else if (transform.transform.position.y <= -verticalBound)
        {
            transform.position = new Vector3(transform.position.x, -verticalBound, 0);
        }

        transform.Translate(direction * mouseSpeed * Time.deltaTime, Space.World);
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            EnemyHealth playerHealth = gameObject.GetComponent<EnemyHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemy.GetBodyDamage());

            }
        }
      
    }

}
