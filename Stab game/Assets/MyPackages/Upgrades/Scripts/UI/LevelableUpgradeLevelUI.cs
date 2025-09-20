using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.FormattableString;
public class LevelableUpgradeLevelUI : MonoBehaviour
{
    [SerializeField] RectTransform _levelImgsParent;
    [SerializeField] TMP_Text _descriptionTextField;
    [SerializeField] TMP_Text _costTextField;
    [SerializeField] LevelablleUpgradeLevelIndicator _levelIndicatorPrefab;
    [SerializeField] public List<LevelablleUpgradeLevelIndicator> _levelIndicators= new List<LevelablleUpgradeLevelIndicator>();

    private void Start()
    {
        //_levelIndicators.AddRange(_levelImgsParent.GetComponentsInChildren<LevelablleUpgradeLevelIndicator>());
    }
    public void SetPreviewLevel(int previewLevel)
    {
        for(int i=0;i<_levelIndicators.Count;i++)
        {
            if (i < previewLevel) _levelIndicators[i].SetPreviewImage(true);
            else _levelIndicators[i].SetPreviewImage(false);
        }
    }
    public void SetUpgradeBuyLevel(int upgradeLevel)
    {
        for (int i = 0; i < upgradeLevel; i++)
        {
            _levelIndicators[i].SetPreviewImage(false);
            _levelIndicators[i].SetFillImage(true);
        }
    }
    public void SetUp(LevelableUpgradeSO upgradeSO,string upgradableAmount,int currentUpgradeLevel)
    {
        
        _levelIndicators.Clear();
        int indicatorsToSpawn = upgradeSO.MaxLevel- _levelImgsParent.childCount;
        _levelIndicators.AddRange(_levelImgsParent.GetComponentsInChildren<LevelablleUpgradeLevelIndicator>());
        _descriptionTextField.text = $"Upgrades {upgradeSO.UpgradeDescription} by {upgradableAmount} per level";
        _costTextField.text = Invariant($"{currentUpgradeLevel * upgradeSO.CostPerLevel} $");
        for(int i=0;i< indicatorsToSpawn; i ++)
        {
            LevelablleUpgradeLevelIndicator indicator= Instantiate(_levelIndicatorPrefab, _levelImgsParent);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_levelImgsParent);
            _levelIndicators.Add(indicator);
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_descriptionTextField);
        UnityEditor.EditorUtility.SetDirty(_costTextField);
#endif
    }
    public void SetPrice(float value)
    {
        _costTextField.text = value.ToString("0.00", CultureInfo.InvariantCulture);
    }
    private void Reset()
    {
        _descriptionTextField.text = $"Upgrades dfs by wwwww per level";
    }
}
