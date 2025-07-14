using GLTFast.Schema;
using UnityEngine;

public class GroupProjectile : MonoBehaviour
{
    public int damage = 10;
    private int attackID;

    public float speed;

    [SerializeField] private float horizontalBound;
    [SerializeField] private float verticalBound;

  
    private void OnEnable()
    {
        attackID = AttackIDGenerator.GetNextID();
        //find all children and set active for all  
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);


        Vector3 pos = transform.position;
        if (pos.y >= verticalBound || pos.y < -verticalBound || pos.x >= horizontalBound || pos.x < -horizontalBound)
        {
            ObjectPoolManager.ReturnObject(gameObject);
        }
    }

    public void DoDamage(EnemyHealth target)
    {
            target.TakeDamage(damage,attackID); 
    }
    public void DoDamage(BossHealth target)
    {
        target.TakeDamage(damage,attackID);
    }
    public void DoDamage(Rocket target)
    {
        target.TakeDamage(damage);
    }
}
public static class AttackIDGenerator
{
    private static int currentID = 0;

    public static int GetNextID()
    {
        currentID++;
        if (currentID == 100) currentID = 1; // Wrap around
        return currentID;
    }
}
