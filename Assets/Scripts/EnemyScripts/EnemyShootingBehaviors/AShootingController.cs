using UnityEngine;
 public abstract class AShootingController:ScriptableObject 
{
    public abstract void Shoot(Transform shooterTransform, Transform playerPos, GameObject projectilePrefab);
}
