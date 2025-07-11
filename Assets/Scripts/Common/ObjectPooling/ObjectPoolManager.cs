using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PoolObjectInfo> objectPools = new List<PoolObjectInfo>();

    public static PoolType poolType;
    private GameObject _objectPoolEmptyHolder;
    private static GameObject playerProjectilePool;
    private static GameObject enemyProjectilePool;
    private static GameObject particle;  
    public enum PoolType
    {
        PlayerProjectile,
        EnemyProjectile,
        Particle,
        None,
    }
    private void Awake()
    {
        SetUpEmpty();
    }

    private void SetUpEmpty()
    {
        _objectPoolEmptyHolder = new GameObject("PoolObjects");

        playerProjectilePool = new GameObject("PlayerProjectilePool");
        playerProjectilePool.transform.SetParent(_objectPoolEmptyHolder.transform);

        enemyProjectilePool = new GameObject("EnemyProjectilePool");
        enemyProjectilePool.transform.SetParent(_objectPoolEmptyHolder.transform);

        particle=new GameObject("ParticlePool");
        particle.transform.SetParent(_objectPoolEmptyHolder.transform); 
    }

    public static GameObject SpawnObject(GameObject gameObject, Vector3 spawnPos, Quaternion rotation,PoolType poolType=PoolType.None)
    {
        PoolObjectInfo pool = objectPools.Find(x => x.poolName == gameObject.name);
        if (pool == null)
        {
            pool = new PoolObjectInfo();
            pool.poolName = gameObject.name;
            objectPools.Add(pool);

        }
        GameObject obj = pool.poolObjects.FirstOrDefault();
        if (obj == null)
        {
            GameObject parent = SetParentGameObject(poolType);
            obj = Instantiate(gameObject, spawnPos, rotation);
            if (parent != null)
            {
                obj.transform.SetParent(parent.transform);
            }
        }
        else
        {
            obj.transform.position = spawnPos;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            pool.poolObjects.Remove(obj);
        }
        return obj;
    }
    public static void ReturnObject(GameObject gameObject)
    {
        string name = gameObject.name.Substring(0, gameObject.name.Length - 7); // Remove "(Clone)" from the name
        PoolObjectInfo pool = objectPools.Find(x => x.poolName == name);
        if (pool != null)
        {
            gameObject.SetActive(false);
            pool.poolObjects.Add(gameObject);
        }
        else
        {
            Debug.LogWarning("Pool not found for " + gameObject.name);
        }
    }
    private static GameObject SetParentGameObject(PoolType type)
    {
        switch (type)
        {
            case PoolType.PlayerProjectile:
                return playerProjectilePool;

            case PoolType.EnemyProjectile:
                return enemyProjectilePool;
            case PoolType.Particle:
                return particle;    
            case PoolType.None:
                return null;
            default:
                return null;

        }
    }

}
public class PoolObjectInfo
{
    public string poolName;
    public List<GameObject> poolObjects = new List<GameObject>();
}
