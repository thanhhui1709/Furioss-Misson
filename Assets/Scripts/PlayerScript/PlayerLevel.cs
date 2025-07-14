using System;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public int level = 0;
    public int experience = 0;
    public int experienceToNextLevel;
    public LevelData levelData;
    public TextMeshProUGUI levelText;
    public Slider experienceBar;
    public AudioClip levelUpSound;
    private AudioSource audioSource;
    private PlayerWeapon playerWeapon;

    void Awake()
    {
        playerWeapon = GetComponent<PlayerWeapon>();
        GameEvent.instance.onPlayerLevelUp.AddListener(LevelUp);
        GameEvent.instance.onEnemyDie += GainExperience;
        experienceToNextLevel = levelData.getExperienceForLevel(level);
        levelText.text = (level + 1).ToString();
        UpdateExperienceBar();
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void GainExperience(int amount)
    {
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

        bool isMaxLevel = levelData.checkMaxLevel(level);
        if (isMaxLevel)
            return;
        //Up level
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
                break;
           case 10:
                playerWeapon.LevelUpCurrentWeapon();
                break;
            default:
                playerWeapon.IncreaseCurrentWeaponDamage(5);
                break;
        }
        //update text and experience bar
        levelText.text = level.ToString();
        experienceToNextLevel = levelData.getExperienceForLevel(level);
        UpdateExperienceBar();
        //reset experience
        experience = 0;
        //play audio
        audioSource.PlayOneShot(levelUpSound);

    }

    public void UpdateExperienceBar()
    {
        if (experienceBar != null)
        {
            experienceBar.value = (float)experience / experienceToNextLevel;
        }
    }
}
