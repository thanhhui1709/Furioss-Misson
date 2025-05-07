using UnityEngine;

public class EnemyController : EnemyBase
{



    void Start()
    {

        // Tìm player theo tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransfrom = player.transform;
        InvokeRepeating("Shotting", timePerShot+3,timePerShot);
    }

    void Update()
    {
       
    }

}
