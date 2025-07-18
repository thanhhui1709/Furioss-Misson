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
    public AudioClip explosionSound;
    public AudioClip rocketSound;
    private Rigidbody2D rb;
    private AudioSource audioSource;



    private float cooldownTime;

    void Awake()
    {
        cooldownTime = lifetime;
     
        rb = GetComponent<Rigidbody2D>();
        audioSource=GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        cooldownTime = lifetime;
     
        rb.linearVelocity = Vector2.zero;
   
        audioSource.PlayOneShot(rocketSound, 0.5f);
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
               
                audioSource.PlayOneShot(explosionSound,1.5f);

            }
        }
        ObjectPoolManager.SpawnObject(explosionEffect, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);

        StartCoroutine(DelayedReturn());

    }
    private IEnumerator DelayedReturn()
    {
        yield return new WaitForSeconds(0.3f); 
        ObjectPoolManager.ReturnObject(gameObject);
    }
    private void Moving()
    {
        rb.linearVelocity = transform.up * speed;
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Explosion();
        }
    }
}
