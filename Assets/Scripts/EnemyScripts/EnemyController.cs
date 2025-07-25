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
        public AShootingController shootingBehavior;
    }
    private Transform playerTransform;
    public EnemyBase enemyStat;  
    private void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            InvokeRepeating("Shooting", Random.Range(enemyStat.minTimeInitShot, enemyStat.maxTimeInitShot), Random.Range(enemyStat.minTimePerShot, enemyStat.maxTimePerShot));
        }
    }
    private void OnDisable()
    {
        CancelInvoke("Shooting");
    }

    void Update()
    {

    }

 
    private void Shooting()
    {

        if (enemyStat.shootingBehavior != null)
        {
            enemyStat.shootingBehavior.Shoot(this,transform, playerTransform, enemyStat.projectTile);
            if (enemyStat.shootSound != null)
            {
                ObjectPoolManager.PlayAudio(enemyStat.shootSound,1f);
            }
        }

    }
    public int GetBodyDamage() => enemyStat.bodyDamage;
   

}
