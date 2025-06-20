using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [System.Serializable]
    public class Weapon
    {
        public List<GameObject> projectile;
        public int damage;
        public int currentLevel;

        public void LevelUp()
        {
            if (currentLevel < projectile.Count - 1)
            {
                currentLevel++;
             
            }

            damage += 5;
        }
    }

    [SerializeField] private Vector3 offsetBetweenProjectileAndHead;
    public List<Weapon> guns;

    public float fireRate = 10f; // Projectiles per second
    private float fireCooldown;  // Time between shots
    private float cooldown;
    public int currentWeapon;

    void Awake()
    {
        currentWeapon = 0;
        fireCooldown = int.MinValue;
        cooldown = 1 / fireRate;
    }

    void FixedUpdate()
    {
        fireCooldown -= Time.fixedDeltaTime;
        if (Input.GetMouseButton(0) && fireCooldown <= 0)
        {
            Shooting();
            fireCooldown = cooldown;
        }
    }

    private void Shooting()
    {
        Weapon currentWp = guns[currentWeapon];
        GameObject projectile = currentWp.projectile[currentWp.currentLevel];
        Vector3 spawnPosition = transform.position + offsetBetweenProjectileAndHead;
        Instantiate(projectile, spawnPosition, Quaternion.identity);
    }

    public void LevelUpCurrentWeapon()
    {
        guns[currentWeapon].LevelUp();
    }
}
