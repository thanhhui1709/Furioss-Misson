using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
   
    public int numberOfLive;
    public float respawnTime;
    public static GameManager instance;
    public GameEvent GameEvent;
   


    public class Data
    {

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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

    }
    public void LoadGame()
    {


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
