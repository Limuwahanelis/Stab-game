using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine.Search;

// Change "PoolItem" to type of object to make pool for
public class  ItemSpawner: MonoBehaviour
{
    public List<SpawnableItem> Spawneditems => _spawnedItems;
    [SerializeField,SearchContext("t:SpawnableItem")] GameObject _itemPrefab;
    private ObjectPool<SpawnableItem> _pool;
    private List<SpawnableItem> _spawnedItems;
    // Start is called before the first frame update
    void Awake()
    {
        _pool = new ObjectPool<SpawnableItem>(CreateItem, OnTakeItemFromPool, OnReturnItemToPool);
    }

    public SpawnableItem GetItem()
    {
        return _pool.Get();
    }
    SpawnableItem CreateItem()
    {
        SpawnableItem item = Instantiate(_itemPrefab).GetComponent<SpawnableItem>();
        _spawnedItems.Add(item);
        item.SetPool(_pool);
        return item;

    }
    void OnTakeItemFromPool(SpawnableItem item)
    {
        item.gameObject.SetActive(true);
    }
    void OnReturnItemToPool(SpawnableItem item)
    {
        item.OnReturnToPool();
        item.gameObject.SetActive(false);
    }
}

