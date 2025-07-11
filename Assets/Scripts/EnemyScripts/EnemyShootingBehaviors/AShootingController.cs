using UnityEngine;
 public abstract class AShootingController:ScriptableObject 
{
    public abstract void Shoot(MonoBehaviour runner,Transform shooterTransform, Transform playerPos, GameObject projectilePrefab);
}
