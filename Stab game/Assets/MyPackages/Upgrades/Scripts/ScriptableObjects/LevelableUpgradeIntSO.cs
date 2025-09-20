using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Upgrades/Levelable upgrade int")]
public class LevelableUpgradeIntSO : LevelableUpgradeSO
{
    public int PerLevelIncrease { get => _perLevelIncrease; }
    [SerializeField] int _perLevelIncrease;
}