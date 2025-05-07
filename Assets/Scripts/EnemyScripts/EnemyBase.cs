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
    protected Transform playerTransfrom;
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
    protected void Shotting()
    {

        RotateTowardPlayer(playerTransfrom);
        Instantiate(projectTile,transform.position,transform.rotation);
        
    }
    private void RotateTowardPlayer(Transform playerTransform)
    {
        // Tính vector hướng từ enemy -> player
        Vector2 direction = playerTransform.position - transform.position;
        direction.Normalize(); // Đảm bảo độ dài = 1

        // Tính góc giữa hướng lên (Vector2.up) và direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Xoay enemy
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}
