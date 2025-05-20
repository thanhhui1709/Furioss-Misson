using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "ScriptableObjects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public List<WaveData> waveData;
    public float delayBetweenWaveData;
    [HideInInspector]
    public bool isDoneSpawned=false;


    [System.Serializable]
    public class WaveData
    {
        public GameObject enemyPrefab;
        public int numberPerWave;
        public float offset;
        public float delaySpawnPrefab;
    }



}
