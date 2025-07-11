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
    void Start()
    {
        disableMovement = true;
        StartCoroutine(enableMovement());
        transform.position = new Vector3(0,0,0);
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

        // Clamp position within bounds
        float clampedX = Mathf.Clamp(transform.position.x, -horizontalBound, horizontalBound);
        float clampedY = Mathf.Clamp(transform.position.y, -verticalBound, verticalBound);
        transform.position = new Vector3(clampedX, clampedY, 0);

        // Move player
        transform.Translate(direction * mouseSpeed * Time.fixedDeltaTime, Space.World);

        //// 🔄 Rotate player based on horizontal direction
        //float maxTiltAngle = 50f;
        //float tilt = Mathf.Lerp(0, maxTiltAngle, Mathf.Abs(direction.x)) * Mathf.Sign(direction.x);
        //transform.rotation = Quaternion.Euler(0, tilt, 0); // Negative to tilt visually left/right
    }
    IEnumerator enableMovement()
    {
        yield return new WaitForSeconds(5f);
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
