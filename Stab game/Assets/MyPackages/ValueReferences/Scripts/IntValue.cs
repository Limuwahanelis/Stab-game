using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Values/Int")]
public class IntValue : ScriptableObject
{
    public int value;
    public UnityEvent<int> OnValueChanged;
    public void SetValue(float newValue)
    {
        value = ((int)newValue);
        OnValueChanged?.Invoke(value);
    }
}
