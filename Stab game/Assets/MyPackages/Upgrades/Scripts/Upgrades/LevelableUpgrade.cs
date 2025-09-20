using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LevelableUpgradeLevelUI))]
public abstract class LevelableUpgrade : MonoBehaviour
{
    public UnityEvent<LevelableUpgrade,LevelableUpgradeSO, int> OnTryBuyUpgrade;
    [SerializeField] protected LevelableUpgradeLevelUI _upgradeLevellUI;
    protected int _upgradelevelToBuy=0;
    protected int _upgradeCurrentLevel = 0;
    protected float toPay = 0;
    public void IncreaseLevelToBuy(LevelableUpgradeSO upgrade)
    {
        if (_upgradeCurrentLevel >= upgrade.MaxLevel) return;
        _upgradelevelToBuy++;
        _upgradelevelToBuy = math.clamp(_upgradelevelToBuy, _upgradeCurrentLevel + 1, upgrade.MaxLevel);

        float pay = 0;
        for (int i = _upgradeCurrentLevel + 1; i <= _upgradelevelToBuy; i++)
        {
            pay += i * upgrade.CostPerLevel;
        }
        toPay = pay;
        _upgradeLevellUI.SetPreviewLevel(_upgradelevelToBuy);
        _upgradeLevellUI.SetPrice(toPay);
    }
    public void DecreaseLevelToBuy(LevelableUpgradeSO upgrade)
    {
        if (_upgradeCurrentLevel >= upgrade.MaxLevel) return;
        _upgradelevelToBuy--;
        _upgradelevelToBuy = math.clamp(_upgradelevelToBuy, _upgradeCurrentLevel + 1, upgrade.MaxLevel);
        float pay = 0;
        for (int i = _upgradeCurrentLevel + 1; i <= _upgradelevelToBuy; i++)
        {
            pay += i * upgrade.CostPerLevel;
        }
        toPay = pay;
        _upgradeLevellUI.SetPreviewLevel(_upgradelevelToBuy);
        _upgradeLevellUI.SetPrice(toPay);
    }
    protected void TryBuyUpgrade(LevelableUpgradeSO upgrade)
    {
        if (_upgradeCurrentLevel >= upgrade.MaxLevel) return;
       
        OnTryBuyUpgrade?.Invoke(this,upgrade, _upgradeCurrentLevel);
      
    }
    public void ConfirmBuy(LevelableUpgradeSO upgrade)
    {
        _upgradeCurrentLevel = _upgradelevelToBuy;
        _upgradeLevellUI.SetUpgradeBuyLevel(_upgradelevelToBuy);
        IncreaseLevelToBuy(upgrade);
    }
    public abstract void GetCurrentUpgradeLevel();
}
