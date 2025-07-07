using UnityEngine;
using static PlayerHealth;
using System.IO;
using static PlayerWeapon;
using static StageManager;

public class SaveSystem
{
    public static SaveData _SaveData;

    [System.Serializable]
    public struct SaveData
    {
        public PlayerHealthSaveData healthSaveData;
        public PlayerWeaponData weaponData;
        public StageData stageData;

    }
    private static string SaveFileName()
    {
        return Application.persistentDataPath + "/save.json";
    }

    public static void Save()
    {
        HandleSaveData();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_SaveData, true));
    }
    private static void HandleSaveData()
    {
        GameManager.instance.PlayerHealth.Save(ref _SaveData.healthSaveData);
        GameManager.instance.PlayerWeapon.Save(ref _SaveData.weaponData);
        GameManager.instance.StageManager.Save(ref _SaveData.stageData);
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
    private static void HandleLoadData()
    {
        //GameManager.instance.PlayerHealth.Load(_SaveData.healthSaveData);
        GameManager.instance.PlayerWeapon.Load(_SaveData.weaponData);
        //GameManager.instance.StageManager.Load(_SaveData.stageData);
    }


}
