using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/LaserBeam")]
public class ShootLaser : AShootingController
{
    public float offset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        Vector2 dir = (shooterTransform.position - playerPos.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg +90f ;
        shooterTransform.rotation = Quaternion.Euler(0, 0, angle);
        Vector3 spawnPos = shooterTransform.position + shooterTransform.up * offset;

        GameObject.Instantiate(projectilePrefab, spawnPos, shooterTransform.rotation);

    }
}
