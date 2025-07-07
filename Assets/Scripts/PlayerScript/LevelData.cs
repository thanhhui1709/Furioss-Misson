using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
  
    public List<LevelInfo> lv;
    [System.Serializable]
    public class LevelInfo
    {
        public int level;
        public int exp;
     
    }
    public int getExperienceForLevel(int level)
    {
        foreach (var levelInfo in lv)
        {
            if (levelInfo.level == level)
            {
                return levelInfo.exp;
            }
        }
        return 0; // Return 0 if the level is not found
    }
    public bool checkMaxLevel(int level)
    {
       return level == lv[lv.Count-1].level;  
    }
}
