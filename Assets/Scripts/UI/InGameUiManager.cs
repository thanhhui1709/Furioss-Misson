using System.Collections;
using UnityEngine;

public class InGameUiManager : MonoBehaviour
{
    public Canvas UI;
    
    private void OnEnable()
    {
        StartCoroutine(WaitAndSubscribe());
    }
    private void OnDisable()
    {
        GameManager.instance.GameEvent.onStageClear.RemoveListener(DisplayClearStage);
        GameManager.instance.GameEvent.onGameOver.RemoveListener(DisplayGameOver);
    }
    private IEnumerator WaitAndSubscribe()
    {
        yield return new WaitUntil(() => GameManager.instance != null && GameManager.instance.GameEvent != null);
        GameManager.instance.GameEvent.onStageClear.AddListener(DisplayClearStage);
        GameManager.instance.GameEvent.onGameOver.AddListener(DisplayGameOver);
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
