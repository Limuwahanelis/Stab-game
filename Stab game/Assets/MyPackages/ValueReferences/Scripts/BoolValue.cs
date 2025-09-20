using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Values/Bool")]
public class BoolValue : ScriptableObject
{
    public bool value;
    public UnityEvent<bool> OnValueChanged;

    public void SetValue(bool value)
    {
        this.value = value;
        OnValueChanged?.Invoke(value);
    }
}
