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
        public int fireRate;
        public AudioClip gunSound;

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
    public AudioSource src;
    public ParticleSystem muzzleFlash;
 

    private float fireCooldown;  // Time between shots
    private float cooldown;
    public int currentWeapon;

    void Awake()
    {
        GameManager.instance.PlayerWeapon = this;
        SetCurrentGun(0);
        fireCooldown = int.MinValue;
        cooldown = (float)1 / guns[currentWeapon].fireRate;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
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
        Instantiate(projectile, spawnPosition, Quaternion.Euler(0, 0, 90));
        src.PlayOneShot(currentWp.gunSound);
        muzzleFlash.Play();
    }
    private void SetCurrentGun(int index)
    {
        currentWeapon = index;
        src.clip = guns[index].gunSound;
    }

    public void LevelUpCurrentWeapon()
    {
        guns[currentWeapon].LevelUp();
    }

    [System.Serializable]
    public struct PlayerWeaponData
    {
        public List<Weapon> weapons;
        public int currentWeapon;
    }


    public void Save(ref PlayerWeaponData data)
    {
        data.weapons = guns;
        data.currentWeapon = this.currentWeapon;
    }
    public void Load(PlayerWeaponData data)
    {
        guns = data.weapons;
        currentWeapon = data.currentWeapon;
    }
}
