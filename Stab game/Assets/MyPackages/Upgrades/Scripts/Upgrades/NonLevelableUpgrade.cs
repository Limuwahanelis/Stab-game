using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NonLevelableUpgrade : MonoBehaviour,ISerializationCallbackReceiver
{
    public UnityEvent<NonLevelableUpgradeSO> OnUpgradeBought;
    [SerializeField] NonLevelableUpgradeSO _upgrade;
    [SerializeField] NonLevelableUpgradeUI _upgradeUI;
    private NonLevelableUpgradeSO _upgradeDump;
    public  void TryBuyUpgrade()
    {
        OnUpgradeBought?.Invoke(_upgrade);
        _upgradeUI.SetAsBought();
    }
    public void ConfirmBuy()
    {
        _upgradeUI.SetAsBought();
    }

    private void Reset()
    {

        if (_upgradeUI == null)
        {
            _upgradeUI = GetComponent<NonLevelableUpgradeUI>();
        }
        _upgradeUI.SetUp(_upgrade);
#if UNITY_EDITOR
        // Mark scene dirty so the change is saved
        UnityEditor.EditorUtility.SetDirty(_upgradeUI);
#endif
    }
    public void CheckStatus()
    {
        if(UpgradesManager.GetUpgradeStatus(_upgrade.Id))_upgradeUI.SetAsBought();
    }
    public void OnBeforeSerialize()
    {
        _upgradeDump = _upgrade;

        
    }
    public void OnAfterDeserialize()
    {
        if (_upgradeDump != default)
        {
            _upgrade = _upgradeDump;
        }

    }

}
