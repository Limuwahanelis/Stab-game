using Unity.Mathematics;
using UnityEngine;

public class LevelableUpgradeFloat : LevelableUpgrade,ISerializationCallbackReceiver
{
    
    [SerializeField] LevelableUpgradeFloatSO _upgrade;
    private LevelableUpgradeFloatSO _upgradeDump;
  
    public void BuyUpgrade()
    {
        TryBuyUpgrade(_upgrade);
    }

    public void DecreaseLevelToBuy()
    {
        DecreaseLevelToBuy(_upgrade);
    }

    public void IncreaseLevelToBuy()
    {
        IncreaseLevelToBuy(_upgrade);
    }
    private void Reset()
    {
        if (_upgradeLevellUI == null)
        {
            _upgradeLevellUI = GetComponent<LevelableUpgradeLevelUI>();
        }
        if (_upgrade != null)
        {
            _upgradeLevellUI.SetUp(_upgrade, $"{_upgrade.PerLevelIncrease * 100}%", _upgradeCurrentLevel+1);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_upgradeLevellUI);
#endif
        }
    }

    public void OnAfterDeserialize()
    {
        if (_upgrade == null)
        {
            if (_upgradeDump != default)
            {
                _upgrade = _upgradeDump;
            }
        }

    }

    public void OnBeforeSerialize()
    {
        _upgradeDump = _upgrade;
    }
    public override void GetCurrentUpgradeLevel()
    {
        _upgradeCurrentLevel = UpgradesManager.GetUpgradeLevel(_upgrade.Id);
        _upgradelevelToBuy = _upgradeCurrentLevel + 1;
        if (_upgradeCurrentLevel == _upgrade.MaxLevel) _upgradelevelToBuy = _upgradeCurrentLevel;
        toPay += _upgrade.CostPerLevel * _upgradelevelToBuy;
        _upgradeLevellUI.SetPreviewLevel(_upgradelevelToBuy);
        _upgradeLevellUI.SetUpgradeBuyLevel(_upgradelevelToBuy - 1);
        _upgradeLevellUI.SetPrice(toPay);
    }
}
