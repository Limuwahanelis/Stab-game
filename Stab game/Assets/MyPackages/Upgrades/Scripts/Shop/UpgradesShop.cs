using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UpgradesShop : MonoBehaviour
{
   
    [SerializeField]List<LevelableUpgrade> _levelableUpgrades;
    [SerializeField] List<NonLevelableUpgrade> _nonLevelableUpgrades;
    [SerializeField] TMP_Text _playerMoneyTextField;
    private void OnEnable()
    {
       // PlayerStats.savedMoney = PlayerStats.savedMoneyAtLevelStart;
        UpgradesManager.SetLevelAtStart();
        UpgradesManager.SetUnlockAtStart();
       
        //_playerMoneyTextField.text = PlayerStats.savedMoney.ToString("0.00", CultureInfo.InvariantCulture) + " $";
    }
    public void BuyLeveleableUpgrade(LevelableUpgrade upgrade ,LevelableUpgradeSO upgradeSO,int level)
    {
        // check for money
        UpgradesManager.IncreaseUpgradeLevel(upgradeSO.Id, level);
        upgrade.ConfirmBuy(upgradeSO);
        //_playerMoneyTextField.text = PlayerStats.savedMoney.ToString("0.00", CultureInfo.InvariantCulture) + " $";
    }
    public void NonLevelableUpgradeBought(NonLevelableUpgrade upgrade,NonLevelableUpgradeSO upgradeSO)
    {
        // check for money
        UpgradesManager.UnlockUpgrade(upgradeSO.Id);
        upgrade.ConfirmBuy();
        //_playerMoneyTextField.text = PlayerStats.savedMoney.ToString("0.00", CultureInfo.InvariantCulture) + " $";
    }
    public void ResetUpgradesLevel()
    {
        UpgradesManager.ResetLevelAtStart();
        UpgradesManager.ReSetUnlockAtStart();
    }
    private void Start()
    {
        foreach (var upgrade in _levelableUpgrades)
        {
            upgrade.GetCurrentUpgradeLevel();
        }
        foreach (var upgrade in _nonLevelableUpgrades)
        {
            upgrade.CheckStatus();
        }
    }
}
