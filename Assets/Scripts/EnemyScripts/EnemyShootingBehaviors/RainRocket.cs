using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/ShootingBehavior/RainRocket")]
public class RainRocket : AShootingController
{
    public float duration=5f;
    public float delayBetweenShots=0.25f;
    public float xSpawnRange=17f;
    public float[] ySpawnRange = new float[] { 25f, 31f };
    public float averageTimeToShootPlayer=0.4f;

    private bool _isFinished = false;
    public override bool isFinished => _isFinished;

    public override void Shoot(MonoBehaviour runner, Transform shooterTransform, Transform playerPos, GameObject projectilePrefab)
    {
        runner.StartCoroutine(SpawnRocketCoroutine(projectilePrefab,playerPos));
    }
    IEnumerator SpawnRocketCoroutine(GameObject projectilePrefab,Transform playerPos)
    {
        float elapsedTime = 0f;
        float shotCooldown = 0f;
        float shootPlayerCooldown = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            shotCooldown-=Time.deltaTime;
            shootPlayerCooldown += Time.deltaTime;
            if (shotCooldown <= 0f)
            {
                Vector2 spawnPos=GetRandomSpawnPos(playerPos,ref shootPlayerCooldown);
                ObjectPoolManager.SpawnObject(projectilePrefab, spawnPos, Quaternion.Euler(0,0,180),ObjectPoolManager.PoolType.EnemyProjectile);
                shotCooldown = delayBetweenShots;
            }
            yield return null;
        }
        _isFinished = true;

    }
    private Vector2 GetRandomSpawnPos(Transform playerPos,ref float timer)
    {
        if (timer >= averageTimeToShootPlayer)
        {
            timer = 0;
            new Vector2(playerPos.position.x, playerPos.position.y + 40f);
            return new Vector2(playerPos.position.x, playerPos.position.y + 40f); 
        }
        float x = Random.Range(-xSpawnRange, xSpawnRange);
        float y = Random.Range(ySpawnRange[0], ySpawnRange[1]);
       
        return new Vector2(x, y);
    }
}
