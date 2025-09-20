using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/NonLevelable Upgrade")]
public class NonLevelableUpgradeSO : UpgradeSO
{
#if UNITY_EDITOR
    public bool IsUnlocked => _isUnlocked;
    [SerializeField] bool _isUnlocked;
#endif
    public string UpgradeDescription { get => _upgradeDescription; }
    public float Cost { get => _cost;  }

    [SerializeField] string _upgradeDescription;
    [SerializeField] float _cost;

}