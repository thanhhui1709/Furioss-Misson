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

    public Canvas UI;


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToogleMenu();


        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearStage();


        }

    }
    public void GameOver()
    {
        if (numberOfLive == 0)
        {
            Transform UITransform = UI.transform.Find("UI");
            if (UITransform != null)
            {
                Transform textTransform = UITransform.GetComponentInChildren<Transform>(true).Find("GameOverText");
                GameObject text = textTransform.gameObject;
                bool isActive = text.activeSelf;
                text.SetActive(!isActive);
            }
            

        }
       
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
    public void ToogleMenu()
    {

        Transform UITransform = UI.transform.Find("UI");
        if (UITransform != null)
        {
            Transform menuTransform = UITransform.GetComponentInChildren<Transform>(true).Find("Menu");
            GameObject menu=menuTransform.gameObject;
            bool isActive = menu.activeSelf;
            menu.SetActive(!isActive);
            Time.timeScale = isActive ? 1 : 0;

        }


    }
    public void ClearStage()
    {
        Transform UITransform = UI.transform.Find("UI");
        if (UITransform != null)
        {
            Transform textTransform = UITransform.GetComponentInChildren<Transform>(true).Find("ClearStageText");
            GameObject text=textTransform.gameObject;   
            bool isActive=text.activeSelf;
            text.SetActive(!isActive);
        }
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
