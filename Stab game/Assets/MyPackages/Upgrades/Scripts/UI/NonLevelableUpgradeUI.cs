using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NonLevelableUpgradeUI : MonoBehaviour
{
    [SerializeField] TMP_Text _descriptionTextField;
    [SerializeField] TMP_Text _costTextField;
    [SerializeField] TMP_Text _boughtTextField;
    [SerializeField] GameObject _buyButton;
    private void Start()
    {
        
    }
    public void SetAsBought()
    {
        _boughtTextField.gameObject.SetActive(true);
        _buyButton.gameObject.SetActive(false);
        _costTextField.gameObject.SetActive(false);
    }
    public void SetUp(NonLevelableUpgradeSO upgradeSO)
    {

        _descriptionTextField.text = upgradeSO.UpgradeDescription;
        _costTextField.text = $"{upgradeSO.Cost} $";
#if UNITY_EDITOR
        // Mark scene dirty so the change is saved
        UnityEditor.EditorUtility.SetDirty(_descriptionTextField);
        UnityEditor.EditorUtility.SetDirty(_costTextField);
#endif
    }
    public void SetPrice(float value)
    {
        _costTextField.text = value.ToString("0.00", CultureInfo.InvariantCulture);
    }
}
