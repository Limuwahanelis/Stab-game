using UnityEngine;
using UnityEngine.Events;

public class GameEventUpgradeListener : MonoBehaviour
{
    [SerializeField] GameEventUpgradeSO _gameEvent;
    [SerializeField] UnityEvent<LevelableUpgradeSO, int> _response;
    private void OnEnable()
    {
        _gameEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        _gameEvent.UnRegisterListener(this);
    }
    public void RaiseResponse(LevelableUpgradeSO upgrade, int value)
    {
        _response.Invoke(upgrade,value);
    }
}
