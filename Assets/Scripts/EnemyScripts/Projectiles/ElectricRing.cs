using System.Collections.Generic;
using UnityEngine;

public class ElectricRing : MonoBehaviour
{
    public float expandSpeed = 5f;
    public float maxRadius = 10f;
    public int damage = 10;
    [SerializeField] private GameObject electricRingPrefab;
    private float currentScale = 0f;
    public AudioClip electricSound;
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        transform.localScale = Vector3.zero;
        CastElectricSkill();
    }

    void Update()
    {
        float scaleStep = expandSpeed * Time.deltaTime;
        currentScale += scaleStep;

        transform.localScale = Vector3.one * currentScale;

        if (currentScale >= maxRadius)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !damagedEnemies.Contains(other.gameObject))
        {
            damagedEnemies.Add(other.gameObject);

            if (other.TryGetComponent(out PlayerHealth health))
            {
                health.TakeDamage(damage);
            }
           

        }
    }
    void CastElectricSkill()
    {
        if (audioSource != null && electricSound != null)
        {
            audioSource.PlayOneShot(electricSound);
        }
        ObjectPoolManager.SpawnObject(electricRingPrefab, transform.position, Quaternion.identity,ObjectPoolManager.PoolType.Particle);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentScale);
    }
}
