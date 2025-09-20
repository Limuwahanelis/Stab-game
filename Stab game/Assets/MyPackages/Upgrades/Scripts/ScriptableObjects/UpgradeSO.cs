using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class UpgradeSO : ScriptableObject
{
    public string Id { get { return _id; } private set { _id = value; } }
    [SerializeField, DebugOnly] private string _id;

    public bool IsInitalised { get { return _isInitalised; } private set { _isInitalised = value; } }
    [SerializeField,DebugOnly] private bool _isInitalised;
    private void Reset()
    {
        Init();
    }
    private void Init()
    {
        Debug.Log("in");

#if UNITY_EDITOR

        if (!_isInitalised)
        {
            Id = GUID.Generate().ToString();
            _isInitalised = true;
        }
#endif
        if (!_isInitalised)
        {
            Debug.LogError("Item was not initialised !");
        }
    }
}