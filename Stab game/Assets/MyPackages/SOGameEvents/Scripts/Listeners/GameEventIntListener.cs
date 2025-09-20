using UnityEngine;
using UnityEngine.Events;

public class GameEventIntListener : MonoBehaviour
{
    [SerializeField] GameEventIntSO _gameEvent;
    [SerializeField] UnityEvent<int> _response;
    private void OnEnable()
    {
        _gameEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        _gameEvent.UnRegisterListener(this);
    }
    public void RaiseResponse(int value)
    {
        _response.Invoke(value);
    }
}
