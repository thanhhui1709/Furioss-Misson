using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/SpreadShoot")]
public class SpreadShoot : AShootingController
{
    public float spreadAngle;
    public float offset;
    public bool shootInReverseDirection=false;

    private bool _isFinished = false;
    public override bool isFinished => _isFinished;

    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        for (int i = -1; i <= 1; i++)
        {
            float angle = i * spreadAngle;
            if (shootInReverseDirection)
            {
                angle += 180f; // Reverse direction by adding 180 degrees
            }
            Quaternion rotation = shooterTransform.rotation* Quaternion.Euler(0, 0,angle);
            Vector3 spawnPosition = shooterTransform.up* offset;
            GameObject projectile = ObjectPoolManager.SpawnObject(projectilePrefab, shooterTransform.position+spawnPosition, rotation, ObjectPoolManager.PoolType.EnemyProjectile);

        }
        _isFinished = true; // Mark the shooting as finished after all shots are fired
    }
}
