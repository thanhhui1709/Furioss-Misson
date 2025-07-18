using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static PoolAudioPlayer;


[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class OrientedRocket : MonoBehaviour
{
    [Header("Rocket Settings")]
    public float health = 60f;
    public float speed = 5f;
    public float rotationSpeed = 200f;
    public float lifetime = 5f;
    public int damage = 10;
    public float explosionRadius = 2f;

    [Header("Effects")]
    public GameObject explosionEffect;
    public AudioClip rocketSound;
    public AudioClip explosionSound;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private GameObject target;
    private float timeAlive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        timeAlive = 0f;
        target = GameObject.FindGameObjectWithTag("Player");

        if (rocketSound) ObjectPoolManager.PlayAudio(rocketSound, 1.2f);
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = transform.up * speed;
            return;
        }

        Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized;
        float angle = Vector2.SignedAngle(transform.up, direction);

        // Rotate gradually towards target
        float rotateAmount = Mathf.Clamp(angle, -rotationSpeed * Time.fixedDeltaTime, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation + rotateAmount);

        // Move forward
        rb.linearVelocity = transform.up * speed;

        timeAlive += Time.fixedDeltaTime;
        if (timeAlive > lifetime)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionSound) ObjectPoolManager.PlayAudio(explosionSound, 1.2f);
        if (explosionEffect)
        {
           
            ObjectPoolManager.SpawnObject(explosionEffect, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);
        }
         ObjectPoolManager.ReturnObject(gameObject);
    }
 
    
    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        health -= amount;
        if (health <= 0)
        {
            Explode();
        }
    }
}
