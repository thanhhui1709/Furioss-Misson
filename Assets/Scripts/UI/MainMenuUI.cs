using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button LoadGameButton;
    private void Awake()
    {
        LoadGameButton.onClick.AddListener(()=>GameManager.instance.LoadGame());
    }
    public void StartGameScene()
    {
        SceneManager.LoadScene(1);
      
    }
   
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif  
    }
    public void LoadGame()
    {

    }
   
}
