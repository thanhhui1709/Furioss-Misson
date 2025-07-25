using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static PlayerWeapon;
using System.IO;
using System.Threading.Tasks;


public class GameManager : MonoBehaviour
{

    public int currentSceneIndex;
    public int numberOfLive;
    public float respawnTime;
    public static GameManager instance;

    public PlayerHealth PlayerHealth { get; set; }
    public PlayerWeapon PlayerWeapon { get; set; }
    public StageManager StageManager { get; set; }
    public PlayerLevel PlayerLevel { get; set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        GameEvent.instance.onStageClear.AddListener(ClearStage);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvent.instance.onStageClear.RemoveListener(ClearStage);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneIndex = scene.buildIndex;
        Debug.Log("Scene loaded. Index updated to: " + currentSceneIndex);
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
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex+1;
        StageManager.ResetWaveAfterClearStage();
        SaveGame();
        StartCoroutine(GoToNextScene(SceneManager.GetActiveScene().buildIndex));   
    }
    IEnumerator GoToNextScene(int currentSceneIndex)
    {
        yield return new WaitForSeconds(5f);
        LoadGame();
       
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
    public void SaveSceneData(ref SceneData sceneData)
    {    
        sceneData.sceneIndex = currentSceneIndex;
        sceneData.numberOfLives = numberOfLive;
    }
    public async Task LoadScene(SceneData sceneData)
    {
        await SceneManager.LoadSceneAsync(sceneData.sceneIndex);
        numberOfLive = sceneData.numberOfLives;
    }

    [System.Serializable]
    public struct SceneData
    {
        public int sceneIndex;
        public int numberOfLives;
    }





}
