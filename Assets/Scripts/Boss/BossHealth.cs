using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BossHealth : MonoBehaviour
{

    //health component
    [SerializeField] private float maxHealth;
    public Slider healthBar;
    public Image healthFill;
    public Gradient gradient;
    public GameObject explosion;
    public AudioClip explosionSound;

    //runtime variable
    private bool isVulnerable = true;
    private BossController bossController;
    private float currentHealth;
    private Camera camera;
    private Queue<int> attackIDs = new Queue<int>();
    int MaxAttackIDs = 100;

    public void Initialize(Slider externalHealthBar, Image externalHealthFill)
    {
        healthBar = externalHealthBar;
        healthFill = externalHealthFill;
        healthBar.gameObject.SetActive(true);
        currentHealth = maxHealth;
        UpdateHealthBar();
        camera = GameObject.FindAnyObjectByType<Camera>();
        bossController = GetComponent<BossController>();

    }

    public void TakeDamage(float damage, int attackID)
    {
        if (!isVulnerable)
        {
            Debug.Log("Boss is invulnerable, cannot take damage.");

            return;


        }
        if (attackIDs.Contains(attackID)) return; // prevent duplicate damage from same attack ID
        attackIDs.Enqueue(attackID);
        if (attackIDs.Count > MaxAttackIDs)
        {
            attackIDs.Clear(); 
        }
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
            isVulnerable = false; // make boss invulnerable after death
        }

    }
    private void Die()
    {
        DestroyAllRelevantGameObject();
        if (bossController.checkFinalPhase())
        {
            GameEvent.instance.TriggerStageClearEvent();
            Destroy(transform.root.gameObject);

            return;
        }
        else
        {
            bossController.StopAllCoroutines();
            StartCoroutine(ChangePhase());
        }
        ObjectPoolManager.SpawnObject(explosion, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);
        ObjectPoolManager.PlayAudio(explosionSound, 1f);
        camera.GetComponent<CameraController>().Shake();
        healthFill.enabled = false;


    }
    private void DestroyAllRelevantGameObject()
    {
        List<string> tags = new List<string> { "Enemy", "Rocket", "EnemyProjectile" };
        List<GameObject> objects = new List<GameObject>();
        foreach (string tag in tags)
        {
            objects.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }
        foreach (GameObject obj in objects)
        {
            ObjectPoolManager.ReturnObject(obj);
        }
    }
    private void UpdateHealthBar()
    {
        float value = (float)currentHealth / maxHealth;
        healthBar.value = value;
        healthFill.color = gradient.Evaluate(value);
    }
    IEnumerator ResetHealthAfterPhase()
    {

        yield return new WaitForSeconds(bossController.timeBetweenPhases);
        float elapsedTime = 0f;
        healthFill.enabled = true;
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            float value = Mathf.Lerp(0, 1, elapsedTime / 3f);
            currentHealth = Mathf.Lerp(0, maxHealth, value);
            UpdateHealthBar();
            yield return null;
        }
        isVulnerable = true;
    }
    IEnumerator ChangePhase()
    {
        yield return StartCoroutine(ResetHealthAfterPhase());
        bossController.ReSpawn();
    }


}
