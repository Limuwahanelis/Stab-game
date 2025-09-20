using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelableUpgradeSO : UpgradeSO
{
#if UNITY_EDITOR
    public int CurrentUpgradeLevel=>_currentUpgradeLevel;
    [SerializeField,Header("Debug")] int _currentUpgradeLevel;
#endif

    public int MaxLevel { get => _maxLevel;  }
    public float CostPerLevel { get => _costPerLevel;  }
    public string UpgradeDescription { get => _upgradeDescription;}

    [SerializeField,Space] int _maxLevel;
    [SerializeField] float _costPerLevel;
    [SerializeField] string _upgradeDescription;


    
}