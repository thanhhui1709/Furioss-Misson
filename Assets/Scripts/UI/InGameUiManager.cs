using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUiManager : MonoBehaviour
{
    public static InGameUiManager instance;
    public Canvas UI;

    public Image levelFill;
  
    private void OnEnable()
    {
        StartCoroutine(WaitAndSubscribe());
    
       
    }
     
    private void OnDisable()
    {
        GameEvent.instance.onStageClear.RemoveListener(DisplayClearStage);
        GameEvent.instance.onGameOver.RemoveListener(DisplayGameOver);
    }
    private IEnumerator WaitAndSubscribe()
    {
        yield return new WaitUntil(() => GameManager.instance != null && GameEvent.instance != null);
        GameEvent.instance.onStageClear.AddListener(DisplayClearStage);
        GameEvent.instance.onGameOver.AddListener(DisplayGameOver);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToogleMenu();


        }


    }
    public void ToogleMenu()
    {

        Transform UITransform = UI.transform.Find("UI");
        if (UITransform != null)
        {
            Transform menuTransform = UITransform.GetComponentInChildren<Transform>(true).Find("Menu");
            GameObject menu = menuTransform.gameObject;
            bool isActive = menu.activeSelf;
            menu.SetActive(!isActive);
            Time.timeScale = isActive ? 1 : 0;

        }


    }
    public void DisplayGameOver()
    {

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
    public void ExitToMenu()
    {
        GameManager.instance.SaveGame();
        SceneManager.LoadScene(0);
        Time.timeScale = 1; // Reset time scale to normal when exiting to menu
    }
    public void DisplayClearStage()
    {
        Transform UITransform = UI.transform.Find("UI");
        if (UITransform != null)
        {
            Transform textTransform = UITransform.GetComponentInChildren<Transform>(true).Find("ClearStageText");
            GameObject text = textTransform.gameObject;
            bool isActive = text.activeSelf;
            text.SetActive(!isActive);
        }
    }
}
