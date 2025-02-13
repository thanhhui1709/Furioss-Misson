using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int health;
    public List<GameObject> dropItems;
    public GameObject chickenEgg;
    void Start()
    {
        InvokeRepeating("SpawnEgg", 1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
    }
    private void DropItem()
    {
        int number = Random.Range(1, 100);

        if (dropItems.Count >= 2) // Ensure the list has at least 2 items
        {
            GameObject itemToDrop = number <= 5 ? dropItems[1] : dropItems[0];
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }
    private void SpawnEgg()
    {
        Instantiate(chickenEgg, transform.position, Quaternion.identity);
    }
}
