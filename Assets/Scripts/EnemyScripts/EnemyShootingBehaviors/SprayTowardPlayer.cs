using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/SprayTowardPlayer")]
public class SprayTowardPlayer : AShootingController
{
    public float rotateSpeed = 180f;
    public float fireRate = 10f; // Time in seconds between shots
    public float duration = 4f;
    public Vector3[] offset;
    public AudioClip audioClip;

    private float shootTimer = 0f;
    private bool _isFinished = false;

    public override bool isFinished => _isFinished;

    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        runner.StartCoroutine(SprayTowardPlayerCoroutine(shooterTransform, playerPos, projectilePrefab));
    }
    private void RotateTowardPlayer(Transform shooterTransform, Transform playerPos)
    {
        Vector3 direction = (playerPos.position - shooterTransform.position).normalized;
        Quaternion rotateAngle = Quaternion.LookRotation(Vector3.forward, direction);

        shooterTransform.rotation = Quaternion.RotateTowards(shooterTransform.rotation, rotateAngle, rotateSpeed * Time.deltaTime);


    }
    IEnumerator SprayTowardPlayerCoroutine(Transform transform, Transform playerPos, GameObject projectilePrefab)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            RotateTowardPlayer(transform, playerPos);
            shootTimer += Time.deltaTime;
            if (shootTimer >= (1 / fireRate))
            {
                shootTimer = 0f;
                // Spawn a projectile with a random offset
                Vector3 randomOffSet = offset[Random.Range(0, offset.Length)];
                Debug.Log("Random Offset: " + randomOffSet);
                Vector3 offsetPos = transform.rotation * randomOffSet;

                Vector3 spawnPosition = transform.position + offsetPos;
                GameObject projectile = ObjectPoolManager.SpawnObject(projectilePrefab, spawnPosition, transform.rotation, ObjectPoolManager.PoolType.EnemyProjectile);
                ObjectPoolManager.PlayAudio(audioClip, 1f);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isFinished = true; // Mark the shooting as finished after the duration



    }

}
