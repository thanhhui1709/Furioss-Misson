using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<EnemyWave> enemyWaves;
    private EnemyWave currentWave;
    [SerializeField] private float delayPerWave;
    private int currentWaveIndex = 0;
    void Start()
    {
        currentWave = enemyWaves[currentWaveIndex];
        StartCoroutine(SpawnAllWave());
    }

    // Update is called once per frame
    private void SetNextCurrentWave()
    {
       
            currentWaveIndex++;
            currentWave=enemyWaves[currentWaveIndex];
        
    }
    // instantiate all enemyprefab from 1 wave data
    private IEnumerator SpawnSingleWaveData(EnemyWave.WaveData wave)
    {
        for (int i = 0; i < wave.numberPerWave; i++)
        {
            var enemy = Instantiate(wave.enemyPrefab, wave.enemyPrefab.transform);
            EnemyMovementBySequence embs = enemy.GetComponent<EnemyMovementBySequence>();
            //embs.currentPattern.offset = i * wave.offset;
            embs.currentPattern = new TopSwingPattern() { duration=3f, startX=-0.1f, xRange=6f};
            Debug.Log(embs.currentPattern.ToString());
            yield return new WaitForSeconds(wave.delaySpawnPrefab); // Delay between each enemy in wave
        }
    }
    // instatiate all wave data(1 complete wave)
    private IEnumerator SpawnWave(EnemyWave currentWave)
    {

        foreach (var wave in currentWave.waveData)
        {
            StartCoroutine(SpawnSingleWaveData(wave));
            yield return new WaitForSeconds(currentWave.delayBetweenWaveData);

        }
        currentWave.isDoneSpawned = true;


    }
    // instantiate all enemy wave
    private IEnumerator SpawnAllWave()
    {
        while (true)
        {
            StartCoroutine(SpawnWave(currentWave));
            if (currentWave.isDoneSpawned && currentWaveIndex <= enemyWaves.Count - 1)
            {
                SetNextCurrentWave();
                yield return new WaitForSeconds(delayPerWave);
            }
            break;
        } 
    }
 
}
