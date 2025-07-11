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
        public float minTimePerShot;
        public float maxTimePerShot;
        public float minTimeInitShot = 7f;
        public float maxTimeInitShot = 10f;
        public int bodyDamage;
        public AudioClip shootSound;
        public AShootingController shottingBehavior;
    }
    private Transform playerTransfrom;
    public EnemyBase enemyStat;
    private EnemyHealth enemyHealth;
    private AudioSource audioSource;




    void Start()
    {

        // Tìm player theo tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = gameObject.GetComponentInChildren<EnemyHealth>(true);
        audioSource = GetComponent<AudioSource>();
        if (player != null)
        {
            playerTransfrom = player.transform;
            InvokeRepeating("Shooting", Random.Range(enemyStat.minTimeInitShot,enemyStat.maxTimeInitShot), Random.Range(enemyStat.minTimePerShot, enemyStat.maxTimePerShot));
        }
    }

    void Update()
    {

    }

 
    private void Shooting()
    {

        if (enemyStat.shottingBehavior != null)
        {
            enemyStat.shottingBehavior.Shoot(this,transform, playerTransfrom, enemyStat.projectTile);
            if (enemyStat.shootSound != null)
            {
                audioSource.PlayOneShot(enemyStat.shootSound);
            }
        }

    }
    public int GetBodyDamage() => enemyStat.bodyDamage;
    public EnemyHealth GetEnemyHealth() => enemyHealth;
   

}
