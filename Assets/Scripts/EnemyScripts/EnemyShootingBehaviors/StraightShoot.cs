using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/Straight")]
public class StraightShoot : AShootingController
{
    public int numberOfShot;
    public float delayBetweenShots;

    private bool _isFinished = false;
    public override bool isFinished => _isFinished;

    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        runner.StartCoroutine(ShootCoroutine(shooterTransform, playerPos, projectilePrefab));
    }



    IEnumerator ShootCoroutine(Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        for (int i = 0; i < numberOfShot; i++)
        {
            ObjectPoolManager.SpawnObject(projectilePrefab, shooterTransform.position, shooterTransform.rotation, ObjectPoolManager.PoolType.EnemyProjectile);
            yield return new WaitForSeconds(delayBetweenShots);
        }
        _isFinished = true; // Mark the shooting as finished after all shots are fired
    }
}
