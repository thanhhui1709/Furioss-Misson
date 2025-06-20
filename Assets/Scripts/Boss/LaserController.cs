using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;

public class LaserController : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float dealDamageBetween = 0.25f;

    [SerializeField] private float accumulateTime = 3f;
    [SerializeField] private Vector3 scaleAccumulate;
    [SerializeField] private Vector3 scaleRealease;
    [SerializeField] private float releaseDuration = 3f;
    [SerializeField] private float scaleTime = 0.5f;
    private float damageCooldown;
    private enum LaserState { Accumulate, Release };
    private LaserState state;
    private float timer;

    // Store last time damage was applied to each object
    void Start()
    {
        damageCooldown = int.MinValue;
        state= LaserState.Accumulate;
        timer = 0;
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
                    transform.localScale = Vector3.Lerp(transform.localScale, scaleAccumulate, timer/accumulateTime);
                }
                else
                {
                    timer = 0;
                    state= LaserState.Release;
                }

                    break;



            case LaserState.Release:
                if (timer <= scaleTime) {
                    transform.localScale = Vector3.Lerp(transform.localScale, scaleRealease, timer / scaleTime);

                }
                else if(timer>= releaseDuration)
                {
                    Destroy(gameObject);
                    return;
                }

                    break;

        }



    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&state.Equals(LaserState.Release))
        {
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
