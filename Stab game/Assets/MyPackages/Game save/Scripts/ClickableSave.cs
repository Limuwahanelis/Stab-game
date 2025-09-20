using System.Globalization;
using TMPro;
using UnityEngine;

public class ClickableSave : MonoBehaviour
{
    [SerializeField] TMP_Text _timeTextField;
    [SerializeField] TMP_Text _dateTextField;
    [SerializeField] TMP_Text _saveNumTextField;
    public void AssginSave(GameData gameData, int index)
    {
        _timeTextField.text = HelperClass.FormatSecondsToTime( (int)gameData.timePlayed);
        _dateTextField.text = gameData.date.ToString("dd.MM.yyyy",CultureInfo.InvariantCulture);
        _saveNumTextField.text = $"Save {index}";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
