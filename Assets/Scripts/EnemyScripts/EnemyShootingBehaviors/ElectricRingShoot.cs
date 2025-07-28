using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/ElectricRing")]
public class ElectricRingShoot : AShootingController
{
    public int numberOfRings = 4;
    public float delayBetweenRings = 0.5f;
    public bool _isFinished = false;

    public override bool isFinished { get => _isFinished; }

    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        runner.StartCoroutine(SpawnRingAfterTime(shooterTransform, projectilePrefab));
    }
    IEnumerator SpawnRingAfterTime(Transform shooterTransform, GameObject ring)
    {
        _isFinished = true;
        for (int i = 0; i < numberOfRings; i++)
        {
            ObjectPoolManager.SpawnObject(ring, shooterTransform.position, Quaternion.identity);
            yield return new WaitForSeconds(delayBetweenRings);
        }
    }
}
