using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int health;
    public List<GameObject> dropItems;
    public GameObject projectTile;
    [SerializeField] protected float speed;
    [SerializeField] protected float timePerShot;
    [SerializeField] protected int damage;
    protected Transform playerTransfrom;
    [SerializeField]
    private AShootingController shottingBehavior;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
    }
    public void DoDamage(out int playerHealth)
    {
        GameObject player=GameObject.FindGameObjectWithTag("Player");
        PlayerController playerScript= player.GetComponent<PlayerController>(); 
        playerHealth = playerScript.health;
        playerHealth-=damage;
        if (playerHealth <= 0) { 
           Destroy(player); 
        }
    }
    protected virtual void DropItem()
    {
        int number = Random.Range(1, 100);

        if (dropItems.Count >= 2) // Ensure the list has at least 2 items
        {
            GameObject itemToDrop = number <= 5 ? dropItems[1] : dropItems[0];
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }
    protected void Shooting()
    {

        if (shottingBehavior != null) { 
         shottingBehavior.Shoot(transform,playerTransfrom,projectTile);
        
        }
        
    }
   

}
