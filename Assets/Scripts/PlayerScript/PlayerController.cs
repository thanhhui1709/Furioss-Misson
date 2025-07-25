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

    private Rigidbody2D rb;
    void Start()
    {
        disableMovement = true;
        StartCoroutine(enableMovement());
        transform.position = new Vector3(0,0,0);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
       if(!disableMovement)
        {
            MovePlayer();
        }


    }
    private void MovePlayer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        direction.z = 0;

        float clampedX = Mathf.Clamp(transform.position.x, -horizontalBound, horizontalBound);
        float clampedY = Mathf.Clamp(transform.position.y, -verticalBound, verticalBound);
        Vector3 targetPos = new Vector3(clampedX, clampedY, 0) + mouseSpeed * Time.fixedDeltaTime * direction;

        rb.MovePosition(targetPos);
    }
    IEnumerator enableMovement()
    {
        yield return new WaitForSeconds(2f);
        disableMovement = false;
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
