using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage;
    [SerializeField] private float explosionRadius;
    public GameObject explosionEffect;
    public GameObject indicator;
    public AudioClip explosionSound;
    public AudioClip rocketSound;
    private Rigidbody2D rb;
    private Vector2 startPos;


    private float cooldownTime;

    void Awake()
    {
        cooldownTime = lifetime;
     
        rb = GetComponent<Rigidbody2D>();
     
    }
    private void OnEnable()
    {
        cooldownTime = lifetime;
        startPos = transform.position;
        rb.linearVelocity = Vector2.zero;

       if(rocketSound != null)
        {
            ObjectPoolManager.PlayAudio(rocketSound, 1.5f, this.gameObject);
        }
        if (indicator != null)
        {
            IndicateExplosionPos();
        }
    }

        // Update is called once per frame
        void FixedUpdate()
        {
            cooldownTime -= Time.fixedDeltaTime;


            Moving();






            if (cooldownTime <= 0)
            {
                Explosion();
                return;
            }
        }
    
    private void Explosion()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Assuming the enemy has a method to take damage
                collider.GetComponent<PlayerHealth>().TakeDamage(damage);
               
              

            }
        }
        ObjectPoolManager.PlayAudio(explosionSound, 1.5f, this.gameObject);
        ObjectPoolManager.SpawnObject(explosionEffect, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);

        ObjectPoolManager.ReturnObject(gameObject);

    }
   
    private void Moving()
    {
        rb.linearVelocity = transform.up * speed;
      
    }
    public Vector2 CalculateExplosionPos()
    {
        Vector2 explosionPos = startPos + (Vector2)transform.up * speed * lifetime;
        return explosionPos;
    }
    public void IndicateExplosionPos()
    {
        ObjectPoolManager.SpawnObject(indicator, CalculateExplosionPos(), Quaternion.identity, ObjectPoolManager.PoolType.Particle);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Explosion();
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.DrawLine(transform.position, CalculateExplosionPos());
        Gizmos.DrawWireSphere(CalculateExplosionPos(),explosionRadius);
    }
}
