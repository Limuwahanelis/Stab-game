using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new game event", menuName = "Game event/Game event int")]
public class GameEventIntSO : GameEventSO
{
    private List<GameEventIntListener> _listeners = new List<GameEventIntListener>();
    [SerializeField] int _debugValue;
    public override void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].RaiseResponse(_debugValue);
        }
    }
    public void Raise(int value)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].RaiseResponse(value);
        }
    }
    public void RegisterListener(GameEventIntListener listener)
    {
        _listeners.Add(listener);
    }
    public void UnRegisterListener(GameEventIntListener listener)
    {
        _listeners.Remove(listener);
    }
}