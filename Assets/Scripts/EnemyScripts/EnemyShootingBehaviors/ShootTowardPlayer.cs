using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/ShootToWardPlayer") ]
public class ShootTowardPlayer : AShootingController
{
    [SerializeField]
    private int numberOfShot=1;
    [SerializeField]
    private float shootDelay = 0.75f; // Delay between shots in seconds
    private bool _isFinished = false;
    public override bool isFinished => _isFinished;

    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        Vector2 dir = (playerPos.position - shooterTransform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        shooterTransform.rotation = Quaternion.Euler(0, 0, angle);
        runner.StartCoroutine(ShootCoroutine(shooterTransform, playerPos, projectilePrefab));

    }
    IEnumerator ShootCoroutine(Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        for (int i = 0; i < numberOfShot; i++)
        {

            ObjectPoolManager.SpawnObject(projectilePrefab, shooterTransform.position, shooterTransform.rotation, ObjectPoolManager.PoolType.EnemyProjectile);
            yield return new WaitForSeconds(shootDelay);
        }
        _isFinished = true; // Mark the shooting as finished after all shots are fired
    }


}