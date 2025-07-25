using System;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("Level Data")]
    public int level = 0;
    public int experience = 0;
    private int experienceToNextLevel;
    public LevelData levelData;
    [Header("UI Elements")]
    public TextMeshProUGUI levelText;
    public Slider experienceBar;
    public AudioClip levelUpSound;
    private PlayerWeapon playerWeapon;

    void Awake()
    {
        GameManager.instance.PlayerLevel = this;
        playerWeapon = GetComponent<PlayerWeapon>();
       
    }
    private void OnEnable()
    {
        GameEvent.instance.onPlayerLevelUp.AddListener(LevelUp);
        GameEvent.instance.onEnemyDie += GainExperience;
    }
    private void OnDisable()
    {
        GameEvent.instance.onPlayerLevelUp.RemoveListener(LevelUp);
        GameEvent.instance.onEnemyDie -= GainExperience;
    }
    private void Start()
    {
        experienceToNextLevel = levelData.getExperienceForLevel(level);
        levelText.text = (level).ToString();
        UpdateExperienceBar();
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void GainExperience(int amount)
    {
        if (level == levelData.GetMaxLevel()) return;
        
        int remainExpToLevelUp = experienceToNextLevel - experience;
        int excessExp = amount - remainExpToLevelUp;
        experience += amount;
        UpdateExperienceBar();
        if (experience >= experienceToNextLevel)
        {
            GameEvent.instance.TriggerPlayerLevelUpEvent();
        }
        if (excessExp >= 0)
        {

            GainExperience(excessExp);
        }
    }

    private void LevelUp()
    {
        level++;
        switch(level)
        {
           
            case 3:
                playerWeapon.LevelUpCurrentWeapon();
                break;
          
            case 5:
                playerWeapon.LevelUpCurrentWeapon();
                break;
            case 7:
                playerWeapon.LevelUpCurrentWeapon();
                playerWeapon.IncreaseCurrentWeaponFireRate(3f);
                break;
           case 10:
                playerWeapon.LevelUpCurrentWeapon();
                break;
            default:
                playerWeapon.IncreaseCurrentWeaponDamage(2);
                break;
        }
        //update text and experience bar
        levelText.text = level.ToString();
        experienceToNextLevel = levelData.getExperienceForLevel(level);
        UpdateExperienceBar();
        //reset experience
        experience = 0;
        //play audio
        ObjectPoolManager.PlayAudio(levelUpSound,1f);

    }

    public void UpdateExperienceBar()
    {
        if (experienceBar != null)
        {
            experienceBar.value = (float)experience / experienceToNextLevel;
        }
    }
    public void Save(ref PlayerLevelData data)
    {
        data.level = level;
        data.experience = experience;
    }
    public void Load(PlayerLevelData data)
    {
        level = data.level;
        experience = data.experience;
        experienceToNextLevel = levelData.getExperienceForLevel(level);
        UpdateExperienceBar();
        levelText.text = level.ToString();
    }
    [System.Serializable]
    public struct PlayerLevelData
    {
        public int level;
        public int experience;
    }

}

