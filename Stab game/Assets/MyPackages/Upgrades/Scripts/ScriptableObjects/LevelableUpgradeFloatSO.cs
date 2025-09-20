using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Upgrades/Levelable upgrade float")]
public class LevelableUpgradeFloatSO : LevelableUpgradeSO
{
    public float PerLevelIncrease { get => _perLevelIncrease;}
    [SerializeField] float _perLevelIncrease;

}