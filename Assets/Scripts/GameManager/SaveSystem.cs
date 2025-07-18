using UnityEngine;
using static PlayerHealth;
using System.IO;
using static PlayerWeapon;
using static StageManager;
using static PlayerLevel;
using static GameManager;
using System.Threading.Tasks;

public class SaveSystem
{
    public static SaveData _SaveData;

    [System.Serializable]
    public struct SaveData
    {
        public SceneData sceneData;
        public PlayerHealthSaveData healthSaveData;
        public PlayerWeaponData weaponData;
        public StageData stageData;
        public PlayerLevelData playerLevelData;

    }
    private static string SaveFileName()
    {
        return Application.persistentDataPath + "/save.json";
    }

    public static void Save()
    {
        HandleSaveData();
        Debug.Log("Save data to: " + SaveFileName());
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_SaveData, true));
    }
    private static void HandleSaveData()
    {
        GameManager.instance.SaveSceneData(ref _SaveData.sceneData);
        GameManager.instance.PlayerHealth.Save(ref _SaveData.healthSaveData);
        GameManager.instance.PlayerWeapon.Save(ref _SaveData.weaponData);
        GameManager.instance.StageManager.Save(ref _SaveData.stageData);
        GameManager.instance.PlayerLevel.Save(ref _SaveData.playerLevelData);
    }
    public static void Load()
    {
        string fileName = SaveFileName();
        if (File.Exists(fileName))
        {
            string json = File.ReadAllText(fileName);
            _SaveData = JsonUtility.FromJson<SaveData>(json);

        }
        HandleLoadData();
    }
    private static async void HandleLoadData()
    {
        await HandleLoadScene(_SaveData.sceneData);
        GameManager.instance.PlayerHealth.Load(_SaveData.healthSaveData);
        GameManager.instance.PlayerLevel.Load(_SaveData.playerLevelData);
        GameManager.instance.PlayerWeapon.Load(_SaveData.weaponData);
        GameManager.instance.StageManager.Load(_SaveData.stageData);
    }
    private static async Task HandleLoadScene(SceneData sceneData)
    {
        await GameManager.instance.LoadScene(sceneData);



    }
}
