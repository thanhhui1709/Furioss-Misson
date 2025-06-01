using UnityEngine;

public class EnemyController : EnemyBase
{



    void Start()
    {

        // Tìm player theo tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
       if(player != null)
        {
            playerTransfrom = player.transform;
        }
        InvokeRepeating("Shooting", timePerShot+3,timePerShot);
    }

    void Update()
    {
       
    }

}
