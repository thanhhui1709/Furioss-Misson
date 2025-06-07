using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [System.Serializable]
    public class EnemyBase
    {
        public List<GameObject> dropItems;
        public GameObject projectTile;
        public float timePerShot;
        public int bodyDamage;
      
        public AShootingController shottingBehavior;
    }
    private Transform playerTransfrom;
    public EnemyBase enemyStat;
    



    void Start()
    {

        // Tìm player theo tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
       if(player != null)
        {
            playerTransfrom = player.transform;
        }
        InvokeRepeating("Shooting",enemyStat.timePerShot+3, enemyStat.timePerShot);
    }

    void Update()
    {
       
    }

    //protected virtual void DropItem()
    //{
    //    int number = Random.Range(1, 100);

    //    if (dropItems.Count >= 2) // Ensure the list has at least 2 items
    //    {
    //        GameObject itemToDrop = number <= 5 ? dropItems[1] : dropItems[0];
    //        Instantiate(itemToDrop, transform.position, Quaternion.identity);
    //    }
    //}
    private void Shooting()
    {

        if (enemyStat.shottingBehavior != null)
        {
            enemyStat.shottingBehavior.Shoot(transform, playerTransfrom, enemyStat.projectTile);

        }

    }
    public int GetBodyDamage() => enemyStat.bodyDamage;

}
