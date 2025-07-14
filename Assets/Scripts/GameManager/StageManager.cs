using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Wave")]
    [SerializeField] private float activeTime=7f;
    [SerializeField] private List<EnemyWave> enemyWaves;

    [Header("Boss")]
    public GameObject Boss;
    [SerializeField] private Slider bossHealthBar;
    [SerializeField] private Image bossHealthFill;
    [SerializeField] private float spawnBossCountdown;
    [SerializeField] private Vector3 spawnPos;


    private EnemyWave currentWave;
    private int currentWaveIndex = 0;
    void Start()
    {
        GameManager.instance.StageManager = this;
        GameEvent.instance.TriggerStageStart();
        if (enemyWaves.Count != 0)
        {
            currentWave = enemyWaves[currentWaveIndex];
            StartCoroutine(SpawnAllWaveAndBoss());
        }
        else
        {
            StartCoroutine(SpawnBoss());
        }
    }

    private void SetNextCurrentWave()
    {

        currentWaveIndex++;
        if (currentWaveIndex < enemyWaves.Count)
        {
            currentWave = enemyWaves[currentWaveIndex];

        }
    }
    // instantiate all enemyprefab from 1 wave data
    private IEnumerator SpawnSingleWaveData(EnemyWave.WaveData wave)
    {
        for (int i = 0; i < wave.numberPerWave; i++)
        {

            var enemy = Instantiate(wave.enemyPrefab);


            EnemyMovementBySequence embs = enemy.GetComponent<EnemyMovementBySequence>();

            IMovementPattern pattern = Instantiate(embs.MovementSequence.sequences[0]); // clone the pattern
            pattern.offset = i * wave.offset;
            pattern.Initialize(enemy.transform);

            embs.currentPattern = pattern;

            yield return new WaitForSeconds(wave.delaySpawnPrefab); // Delay between each enemy in wave
        }
        StartCoroutine(SpawnDropItem(wave)); // Spawn drop items after all enemies are spawned
    }
    private IEnumerator SpawnDropItem(EnemyWave.WaveData wave)
    {
        if (wave.dropItems.Count == 0) yield break; // No items to drop
        yield return new WaitForSeconds(wave.dropedTime);
        foreach (var item in wave.dropItems)
        {
            GameObject dropItem = Instantiate(item, new Vector3(Random.Range(-20,20),15,0), Quaternion.identity);
          
        }

    }
    // instatiate all wave data(1 complete wave)
    private IEnumerator SpawnWave(EnemyWave currentWave)
    {
        foreach (var wave in currentWave.waveData)
        {
            if (wave.delayForTheNextWaveData > 0)
            {
                yield return StartCoroutine(SpawnSingleWaveData(wave));
                yield return new WaitForSeconds(wave.delayForTheNextWaveData);
            }
            else
            {
                StartCoroutine(SpawnSingleWaveData(wave));
            }
        }

        currentWave.isDoneSpawned = true;
    }

    // instantiate all enemy wave
    private IEnumerator SpawnAllWaveAndBoss()
    {
        yield return new WaitForSeconds(activeTime); // Wait for the active time before spawning the first wave
        while (currentWaveIndex < enemyWaves.Count)
        {
            yield return StartCoroutine(SpawnWave(currentWave)); // Wait until the wave is done
            yield return new WaitUntil(() => CheckWaveClear(currentWave));
            yield return new WaitForSeconds(currentWave.delayForTheNextWave);
            SetNextCurrentWave();
        }
        if (CheckWaveClear(currentWave))
        {
            StartCoroutine(SpawnBoss());

        }
    }
    private bool CheckWaveClear(EnemyWave currentWave)
    {
        List<GameObject> enemy = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        return enemy.Count == 0 && currentWave.isDoneSpawned;
    }
    private IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(spawnBossCountdown);
        GameObject boss = Instantiate(Boss, spawnPos, Quaternion.identity);
        BossHealth bossHealth = boss.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.Initialize(bossHealthBar, bossHealthFill);
        }
    }

    public void Save(ref StageData stageData)
    {
        stageData.currentWave = currentWave;
        stageData.index = currentWaveIndex;
    }
    public void Load(StageData stageData)
    {

        currentWave = stageData.currentWave;
        currentWaveIndex = stageData.index;
    }


    [System.Serializable]
    public struct StageData
    {
        public EnemyWave currentWave;
        public int index;
    }


}
