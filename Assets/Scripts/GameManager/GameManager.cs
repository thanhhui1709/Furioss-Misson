using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static PlayerWeapon;
using System.IO;
using UnityEditor.Overlays;

public class GameManager : MonoBehaviour
{
   
    public int numberOfLive;
    public float respawnTime;
    public static GameManager instance;
    public Data data;

    public PlayerHealth PlayerHealth { get; set; }
    public PlayerWeapon PlayerWeapon { get; set; }
    public StageManager StageManager { get; set; }



    [System.Serializable]
    public class Data
    {
        [Header("Wave data")]
        public int StageIndex;
        public int CurrentWave;
        [Header("Player Data")]
        public int numberOfLive;
        public float MaxHealth;
        public float CurrentHealth;
        public List<Weapon> weapons;
        public int currentWeapon;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
      {
        if (Input.GetKeyDown(KeyCode.Space)) {

            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadGame(); 
        }
    }


    public void GameOver()
    {
       
       
    }
    public void WinGame()
    {

    }
    public void SaveGame()
    {
        SaveSystem.Save();
    }
    public void LoadGame()
    {
        SaveSystem.Load();
    }
   
    public void ClearStage()
    {
        
    }
    public void HandleRespawn()
    {
        StartCoroutine(ReSpawn());
     
    }
    IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(respawnTime);
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject player = allObjects.FirstOrDefault(x => x.name.Equals("Player"));
        if (player != null) { 
          player.SetActive(true);
          PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.ApplyShieldEffect();
        }
      

    }





}
