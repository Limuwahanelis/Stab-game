using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{

# if UNITY_EDITOR
    public static bool _useCustomUpgradeValues = false;
#endif
    public class NonLevelableUpgradeData
    {
        public string id;
        public bool isUnlocked;
        public bool isUnlockedAtStart;
    }
    public class LevelableUpgradeData
    {
        public string id;
        public int level;
        public int levelAtStart;
    }
    public class UpgradeData
    {
        public string id;
        public bool isUnlocked;
    }
    public static List<LevelableUpgradeData> LevelableUpgradeDatas
    {
        get
        {
            if (_levelableUpgradeDatas == null)
            {
                _levelableUpgradeDatas = new List<LevelableUpgradeData>();
                List<LevelableUpgradeSO> tmp= Resources.LoadAll<LevelableUpgradeSO>($"{ScriptPaths.ResourcesLevelableUpgradesPath}/").ToList();
                foreach (LevelableUpgradeSO upgrade in tmp)
                {
                    _levelableUpgradeDatas.Add(new LevelableUpgradeData() 
                    {
                        id = upgrade.Id,
#if UNITY_EDITOR
                        level=_useCustomUpgradeValues?upgrade.CurrentUpgradeLevel:0,
#else
                        level=0,
#endif

                    });
                }
            }
            return _levelableUpgradeDatas;
        }
    }
    public static List<NonLevelableUpgradeData> NonLevelableUpgradeDatas
    {
        get
        {
            if (_nonLevelableUpgradeDatas == null)
            {
                _nonLevelableUpgradeDatas = new List<NonLevelableUpgradeData>();
                List<NonLevelableUpgradeSO> tmp = Resources.LoadAll<NonLevelableUpgradeSO>($"{ScriptPaths.ResourcesNonLevelableUpgradesPath}/").ToList();
                foreach (NonLevelableUpgradeSO upgrade in tmp)
                {
                    _nonLevelableUpgradeDatas.Add(new NonLevelableUpgradeData() 
                    {
                        id = upgrade.Id,
#if UNITY_EDITOR
                        isUnlocked = _useCustomUpgradeValues?upgrade.IsUnlocked:false,
#else
                        isUnlocked = false,
#endif

                    });
                }
            }
            return _nonLevelableUpgradeDatas;
        }
    }
    private static List<LevelableUpgradeData> _levelableUpgradeDatas;
    private static List<NonLevelableUpgradeData> _nonLevelableUpgradeDatas;
    public static List<UpgradeData> UpgradeDatas;
    public static void SetLevelAtStart()
    {
        if (_levelableUpgradeDatas == null) return;
        foreach(LevelableUpgradeData upgrade in _levelableUpgradeDatas)
        {
            upgrade.levelAtStart = upgrade.level;
        }
    }
    public static void ResetLevelAtStart()
    {
        if (_levelableUpgradeDatas == null) return;
        foreach (LevelableUpgradeData upgrade in _levelableUpgradeDatas)
        {
            upgrade.level = upgrade.levelAtStart;
        }
    }
    public static void SetUnlockAtStart()
    {
        if (_nonLevelableUpgradeDatas == null) return;
        foreach (NonLevelableUpgradeData upgrade in _nonLevelableUpgradeDatas)
        {
            upgrade.isUnlockedAtStart = upgrade.isUnlocked;
        }
    }
    public static void ReSetUnlockAtStart()
    {
        if (_nonLevelableUpgradeDatas == null) return;
        foreach (NonLevelableUpgradeData upgrade in _nonLevelableUpgradeDatas)
        {
            upgrade.isUnlocked = upgrade.isUnlockedAtStart;
        }
    }
    public static void IncreaseUpgradeLevel(string id,int level)
    {
        LevelableUpgradeDatas.Find(x => x.id == id).level = level;
    }
    public static int GetUpgradeLevel(string id)
    {
        return LevelableUpgradeDatas.Find(x => x.id == id).level;
    }
    public static bool GetUpgradeStatus(string id)
    {
        return NonLevelableUpgradeDatas.Find(x => x.id == id).isUnlocked;
    }
    public static void UnlockUpgrade(string id)
    {
        NonLevelableUpgradeDatas.Find(x=>x.id == id).isUnlocked = true;
    }

}
