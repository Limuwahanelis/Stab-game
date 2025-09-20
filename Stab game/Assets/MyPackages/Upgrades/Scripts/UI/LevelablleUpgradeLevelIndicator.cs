using UnityEngine;
using UnityEngine.UI;

public class LevelablleUpgradeLevelIndicator : MonoBehaviour
{
    [SerializeField] Image _fillImage;
    [SerializeField] Image _previewImage;

    public void SetFillImage(bool value)
    {
        _fillImage.enabled = value;
    }
    public void SetPreviewImage(bool value)
    {
        _previewImage.enabled = value;
    }
}
