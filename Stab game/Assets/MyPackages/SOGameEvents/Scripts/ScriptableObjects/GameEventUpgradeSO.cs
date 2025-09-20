using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new game event", menuName = "Game event/Game upgrade event")]
public class GameEventUpgradeSO : GameEventSO
{
    private List<GameEventUpgradeListener> _listeners = new List<GameEventUpgradeListener>();
    [SerializeField] LevelableUpgradeSO _debugUpgrade;
    [SerializeField] int _debugLevelUpgrade;
    public override void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].RaiseResponse(_debugUpgrade, _debugLevelUpgrade);
        }
    }
    public void Raise(LevelableUpgradeSO upgrade, int value)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].RaiseResponse(upgrade, value);
        }
    }
    public void RegisterListener(GameEventUpgradeListener listener)
    {
        _listeners.Add(listener);
    }
    public void UnRegisterListener(GameEventUpgradeListener listener)
    {
        _listeners.Remove(listener);
    }
}