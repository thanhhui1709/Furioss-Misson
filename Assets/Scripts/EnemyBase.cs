using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int health;
    public List<GameObject> dropItems;
    public GameObject chickenEgg;
    [SerializeField] protected float speed;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DropItem();
            Destroy(gameObject);
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
    protected void SpawnEgg()
    {
        Instantiate(chickenEgg, transform.position, Quaternion.identity);
    }
}
