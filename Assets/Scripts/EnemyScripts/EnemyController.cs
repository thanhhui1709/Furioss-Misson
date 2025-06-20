using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    [System.Serializable]
    public class EnemyBase
    {
        public GameObject projectTile;
        public float timePerShot;
        public float timeInitShot = 7f;
        public int bodyDamage;

        public AShootingController shottingBehavior;
    }
    private Transform playerTransfrom;
    public EnemyBase enemyStat;
    private EnemyHealth enemyHealth;
 




    void Start()
    {

        // Tìm player theo tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = gameObject.GetComponentInChildren<EnemyHealth>(true);
   
        if (player != null)
        {
            playerTransfrom = player.transform;
            InvokeRepeating("Shooting", enemyStat.timeInitShot, enemyStat.timePerShot);
        }
    }

    void Update()
    {

    }

 
    private void Shooting()
    {

        if (enemyStat.shottingBehavior != null)
        {
            enemyStat.shottingBehavior.Shoot(transform, playerTransfrom, enemyStat.projectTile);

        }

    }
    public int GetBodyDamage() => enemyStat.bodyDamage;
    public EnemyHealth GetEnemyHealth() => enemyHealth;
   

}
