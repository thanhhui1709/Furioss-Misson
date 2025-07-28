using GLTFast.Schema;
using UnityEngine;

public class GroupProjectile : MonoBehaviour
{

    private int attackID;

    public float speed;
    [SerializeField] private float horizontalBound;
    [SerializeField] private float verticalBound;
    private PlayerWeapon playerWeapon;
    private PlayerHealth playerHealth;

    private void OnEnable()
    {
        attackID = AttackIDGenerator.GetNextID();
        playerWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapon>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
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
        float damage = playerWeapon.CurrentWeapon.damage;
        float lifeSteal = playerWeapon.CurrentWeapon.lifeSteal;
        target.TakeDamage(damage, attackID);
        if (lifeSteal > 0) ApplyLifeStealEffect(damage, lifeSteal);



    }
    public void DoDamage(BossHealth target)
    {
        float damage = playerWeapon.CurrentWeapon.damage;
        float lifeSteal = playerWeapon.CurrentWeapon.lifeSteal;
        target.TakeDamage(damage*0.8f, attackID);
        if (lifeSteal > 0) ApplyLifeStealEffect(damage * 0.8f, lifeSteal);
    }
    public void DoDamage(OrientedRocket target)
    {
        float damage = playerWeapon.CurrentWeapon.damage;
        float lifeSteal = playerWeapon.CurrentWeapon.lifeSteal;
        target.TakeDamage(damage);
        if (lifeSteal > 0) ApplyLifeStealEffect(damage, lifeSteal);
    }
    public void ApplyLifeStealEffect(float damage, float stealRate)
    {
        playerHealth.Health((int)(damage * stealRate));
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
