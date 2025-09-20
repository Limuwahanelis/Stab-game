using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetectable:MonoBehaviour
{
    public UnityEvent<TriggerDetectable> OnDisabled;
    private void OnDisable()
    {
        OnDisabled?.Invoke(this);
    }
}
