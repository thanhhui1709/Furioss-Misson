using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [System.Serializable]
    public class Weapon
    {
        public List<GameObject> projectile;
        public float damage;
        public int currentLevel;
        public float fireRate;
        public float lifeSteal = 0;
        public AudioClip gunSound;

        public void LevelUp()
        {
            if (currentLevel < projectile.Count - 1)
            {
                currentLevel++;
            }

            damage += 5;
        }

        public void IncreaseDamage(int amount)
        {
            damage += amount;
        }
        public void IncreaseFireRate(float amount)
        {
            fireRate += amount;
        }

    }

    [System.Serializable]
    public struct PlayerWeaponData
    {
        public List<Weapon> weapons;
        public int currentWeapon;
    }

    [SerializeField] private Vector3 offsetBetweenProjectileAndHead;
    [SerializeField] private AudioSource src;
    [SerializeField] private ParticleSystem muzzleFlash;

    public List<Weapon> guns;
    public int currentWeaponIndex;

    private float fireCooldown;

    public Weapon CurrentWeapon => guns[currentWeaponIndex];

    private void Awake()
    {
        GameManager.instance.PlayerWeapon = this;
        SetCurrentGun(0);
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1 / CurrentWeapon.fireRate;
        }
    }

    private void SetCurrentGun(int index)
    {
        currentWeaponIndex = Mathf.Clamp(index, 0, guns.Count - 1);
        src.clip = guns[currentWeaponIndex].gunSound;
    }

    private void Shoot()
    {
        var weapon = CurrentWeapon;

        if (weapon.projectile == null || weapon.projectile.Count == 0)
            return;

        GameObject projectile = weapon.projectile[weapon.currentLevel];
        Vector3 spawnPos = transform.position + offsetBetweenProjectileAndHead;

        ObjectPoolManager.SpawnObject(projectile, spawnPos, Quaternion.identity, ObjectPoolManager.PoolType.PlayerProjectile);
        src.PlayOneShot(weapon.gunSound);
        muzzleFlash.Play();
    }

    public void LevelUpCurrentWeapon()
    {
        CurrentWeapon.LevelUp();
    }

    public void IncreaseCurrentWeaponDamage(int amount)
    {
        CurrentWeapon.IncreaseDamage(amount);
    }
    public void IncreaseCurrentWeaponFireRate(float amount)
    {
        CurrentWeapon.IncreaseFireRate(amount);
    }

    public void Save(ref PlayerWeaponData data)
    {
        data.weapons = guns;
        data.currentWeapon = currentWeaponIndex;
    }

    public void Load(PlayerWeaponData data)
    {
        guns = data.weapons;
        SetCurrentGun(data.currentWeapon);
    }
}
