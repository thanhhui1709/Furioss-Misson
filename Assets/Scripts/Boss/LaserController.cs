using UnityEngine;
using System.Collections.Generic;

using System.Collections;

public class LaserController : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float dealDamageBetween = 0.25f;

    public float accumulateTime = 3f;
    public AudioClip accumulateSound;
    [SerializeField] private Vector3 scaleAccumulate;
    [SerializeField] private Vector3 scaleRelease;
    public AudioClip releaseSound;
    public float releaseDuration = 3f;
    [SerializeField] private float scaleTime = 0.5f;
    private float damageCooldown;
    private enum LaserState { Accumulate, Release };
    private LaserState state;
    private float timer;
    private Vector3 initScale;

    // Store last time damage was applied to each object
    private void Start()
    {
        initScale = transform.localScale;
       
    }
    private void OnEnable()
    {
        damageCooldown = int.MinValue;
        transform.localScale = initScale;
        state = LaserState.Accumulate;
        timer = 0;

        ObjectPoolManager.PlayAudio(accumulateSound, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            case LaserState.Accumulate:
                if (timer <= accumulateTime)
                {
                    transform.localScale = Vector3.Lerp(initScale, scaleAccumulate, timer/accumulateTime);
                }
                else
                {
                    timer = 0;
                    state= LaserState.Release;
                    ObjectPoolManager.PlayAudio(releaseSound, 1f);
                }

                    break;



            case LaserState.Release:
                if (timer <= scaleTime) {
                    transform.localScale = Vector3.Lerp(scaleAccumulate, scaleRelease, timer / scaleTime);

                }
                else if(timer>= releaseDuration)
                {   
                    if(transform.parent != null)
                    {
                        transform.SetParent(null);
                        GameObject parent=ObjectPoolManager.SetParentGameObject(ObjectPoolManager.PoolType.EnemyProjectile);
                        transform.SetParent(parent.transform);
                    }
                    ObjectPoolManager.ReturnObject(gameObject);
                    return;
                }

                    break;

        }



    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&state.Equals(LaserState.Release))
        {
            Debug.Log("Laser hit player");
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (damageCooldown <= 0)
            {
                playerHealth.TakeDamage(damage);
                damageCooldown = dealDamageBetween;
            }
            damageCooldown -= Time.deltaTime;


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damageCooldown = int.MinValue;
        }
    }
}
