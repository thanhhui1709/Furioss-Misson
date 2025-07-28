using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 5f;
    [SerializeField] private float verticalBound = 3.8f;
    [SerializeField] private float horizontalBound = 7.5f;
    public bool disableMovement;

    private PlayerHealth playerHealth;
    private float timer;
    private Rigidbody2D rb;
    void Start()
    {
        timer = 0f;
        disableMovement = true;
        playerHealth = GetComponent<PlayerHealth>();
        StartCoroutine(enableMovement());
        transform.position = new Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!disableMovement)
        {
            MovePlayer();
            CheckPlayerOutOfBoundForSeconds(4f); // Check if player is out of bounds for 2 seconds
        }


    }
    private void MovePlayer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        direction.z = 0;

        Vector3 newPos = transform.position + direction * mouseSpeed * Time.fixedDeltaTime;

        float clampedX = Mathf.Clamp(newPos.x, -horizontalBound, horizontalBound);
        float clampedY = Mathf.Clamp(newPos.y, -verticalBound, verticalBound);

        Vector3 targetPos = new Vector3(clampedX, clampedY, 0);
        rb.MovePosition(targetPos);
    }
    IEnumerator enableMovement()
    {
        yield return new WaitForSeconds(2f);
        disableMovement = false;
    }
    private void CheckPlayerOutOfBoundForSeconds(float maxTime)
    {

        if (Mathf.Abs(transform.position.x) > horizontalBound || Mathf.Abs(transform.position.y) > verticalBound)
        {
     
            timer += Time.deltaTime;
            if (timer >= maxTime)
            {
                playerHealth.TakeDamage(1000); // Take damage if out of bounds for too long
            }
        }
        else
        {
            timer = 0f; // Reset timer if player is within bounds
        }
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
    //        EnemyHealth playerHealth = gameObject.GetComponent<EnemyHealth>();
    //        if (playerHealth != null)
    //        {


    //        }
    //    }

    //}

}
