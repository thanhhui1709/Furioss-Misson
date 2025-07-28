using UnityEngine;
using static PlayerHealth;
using System.IO;
using static PlayerWeapon;
using static StageManager;
using static PlayerLevel;
using static GameManager;
using System.Threading.Tasks;
using System;

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
        string json = JsonUtility.ToJson(_SaveData, true);
        string encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        File.WriteAllText(SaveFileName(), encoded);
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
            string encoded = File.ReadAllText(SaveFileName());

          
            string json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encoded));

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
