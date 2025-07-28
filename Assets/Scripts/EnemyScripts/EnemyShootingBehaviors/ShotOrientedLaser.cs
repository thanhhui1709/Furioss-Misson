using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/OrientedLaser")]
public class ShotOrientedLaser : AShootingController
{
    public Vector2 offset;
    public float rotationSpeed = 120f; // Speed at which the shooter rotates towards the player
    public float duration=5f;
    public float holdDuration = 1f; // Time to hold the laser before it starts rotating

    private bool _isFinished=false;
    public override bool isFinished => _isFinished;

    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        RotateTowardPlayerInstant(shooterTransform, playerPos);
        Vector3 spawnPos= shooterTransform.rotation*  offset ;
        GameObject laser = ObjectPoolManager.SpawnObject(projectilePrefab, shooterTransform.position+spawnPos, shooterTransform.rotation);
        // check laser has laser controller or not
        if (laser.TryGetComponent<LaserController>(out LaserController laserController))
        {
            duration=laserController.releaseDuration;
            holdDuration = laserController.accumulateTime;
        }
        if (laser.transform.parent != null)
        {
            laser.transform.SetParent(null,true);
        }
            laser.transform.SetParent(shooterTransform,true);
            runner.StartCoroutine(RotateTowardPlayerCoroutine(shooterTransform, playerPos, laser));
    }
    IEnumerator RotateTowardPlayerCoroutine(Transform shooter,Transform player, GameObject laser)
    {
        float elapsedTime = 0f;
        while(elapsedTime < duration+holdDuration)
        {
            if(elapsedTime>=holdDuration) RotateTowardPlayer(shooter, player);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isFinished = true;
    }
    private void RotateTowardPlayer(Transform shooter,Transform player)
    {
        Vector3 direction=(player.position - shooter.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        shooter.rotation = Quaternion.RotateTowards(shooter.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void RotateTowardPlayerInstant(Transform shooter, Transform player)
    {
        Vector3 direction = (player.position - shooter.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        shooter.rotation = targetRotation;
    }
}
