using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnableItem : MonoBehaviour
{
    private IObjectPool<SpawnableItem> _pool;
    private Coroutine _cor;
    private Action _onReturnToPool;
    private IEnumerator ReturnToPoolCor(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
        _cor = null;
    }
    public void SetActionToReturnToPool(Action toPerform)
    {
        _onReturnToPool = toPerform;
    }
    public void OnReturnToPool()
    {
        if (_onReturnToPool != null)
        {
            _onReturnToPool();
        }
        _onReturnToPool = null;
    }
    public void SetPool(ObjectPool<SpawnableItem> pool) => _pool = pool;
    public void ReturnToPool() => _pool.Release(this);
    public void ReturnToPool(float timeDelay)
    {
        if (_cor != null) return;
        _cor=StartCoroutine(ReturnToPoolCor(timeDelay));
    }
}
