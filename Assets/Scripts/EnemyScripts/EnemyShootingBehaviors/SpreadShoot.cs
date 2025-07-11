using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/SpreadShoot")]
public class SpreadShoot : AShootingController
{
    public float spreadAngle;
    public Vector3 offsetPos;
    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        for (int i = -1; i <= 1; i++)
        {
            float angle = i * spreadAngle;
            Quaternion rotation =  Quaternion.Euler(0, 0, 180+angle);
            Vector3 spawnPosition = shooterTransform.position-offsetPos;
            GameObject projectile = ObjectPoolManager.SpawnObject(projectilePrefab, spawnPosition, rotation, ObjectPoolManager.PoolType.EnemyProjectile);

        }
    }
}
