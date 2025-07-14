using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/ShootToWardPlayer") ]
public class ShootTowardPlayer : AShootingController
{
    [SerializeField]
    private int numberOfShot;
    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        Vector2 dir = (playerPos.position - shooterTransform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        shooterTransform.rotation = Quaternion.Euler(0, 0, angle);
        for (int i = 0; i < numberOfShot; i++)
        {
            ObjectPoolManager.SpawnObject(projectilePrefab, shooterTransform.position, shooterTransform.rotation,ObjectPoolManager.PoolType.EnemyProjectile);

        }

    }


}