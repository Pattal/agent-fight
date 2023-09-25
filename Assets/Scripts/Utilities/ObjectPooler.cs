using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public PoolID ID;
        public ServiceLocator Prefab;
        [Min(0)] public int SizeOfPool;

        [HideInInspector] public List<ServiceLocator> UsedGameObjects = new();
        [HideInInspector] public List<ServiceLocator> ListOfPool = new();

        public bool AllItemsUsed =>
            UsedGameObjects.Count == ListOfPool.Count;
    }

    [SerializeField] private List<Pool> _pools = new();
    [SerializeField] private Transform _parent;
    [SerializeField][Min(1)] private int _defaultValueOfExpand;

    private readonly Dictionary<PoolID, Pool> _poolsDictionary = new();

    public void InitializePools()
    {
        foreach (Pool pool in _pools)
        {
            for (int i = 0; i < pool.SizeOfPool; i++)
            {
                CreatePoolObject(pool);
            }

            _poolsDictionary.Add(pool.ID, pool);
        }
    }

    public ServiceLocator SpawnFromPool(PoolID poolTag, Transform parent)
    {
        if (!CheckIfPoolCreated(poolTag))
            return null;

        ServiceLocator objectToSpawn = SpawnObjectFromPool(poolTag);

        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.SetParent(parent, false);

        return objectToSpawn;
    }

    public ServiceLocator SpawnFromPool(PoolID poolTag, Vector3 at, Quaternion rotation, Transform parent)
    {
        if (!CheckIfPoolCreated(poolTag))
            return null;

        ServiceLocator objectToSpawn = SpawnObjectFromPool(poolTag);

        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.position = at;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.SetParent(parent);

        return objectToSpawn;
    }

    public void DisableGameObjectFromPool(ServiceLocator gameObjectServiceLocator)
    {
        Pool pool = FindGameObjectInPool(gameObjectServiceLocator);

        if (pool == null)
        {
            Debug.LogError("You are trying to disable gameobject that is not from pools");
            return;
        }

        pool.UsedGameObjects.Remove(gameObjectServiceLocator);

        gameObjectServiceLocator.gameObject.SetActive(false);
        gameObjectServiceLocator.Reset();
        gameObjectServiceLocator.gameObject.transform.SetParent(transform, false);
    }

    public void DisableAllActiveObjectsInPool(PoolID poolTag)
    {
        if (!CheckIfPoolCreated(poolTag))
            return;

        foreach (ServiceLocator gameObjectInPool in _poolsDictionary[poolTag].UsedGameObjects)
        {
            gameObjectInPool.gameObject.SetActive(false);
            gameObjectInPool.Reset();
        }

        _poolsDictionary[poolTag].UsedGameObjects.Clear();
    }

    private ServiceLocator CreatePoolObject(Pool pool)
    {
        ServiceLocator prefabOfPool = Instantiate(pool.Prefab, _parent);

        prefabOfPool.gameObject.SetActive(false);
        pool.ListOfPool.Add(prefabOfPool);

        return prefabOfPool;
    }

    private ServiceLocator SpawnObjectFromPool(PoolID poolTag)
    {
        ServiceLocator objectToSpawn = FindNotActiveObject(poolTag);
        _poolsDictionary[poolTag].UsedGameObjects.Add(objectToSpawn);

        return objectToSpawn;
    }

    private ServiceLocator FindNotActiveObject(PoolID poolTag)
    {
        Pool pool = _poolsDictionary[poolTag];

        if (!pool.AllItemsUsed)
            return pool.ListOfPool.FirstOrDefault(x => !x.gameObject.activeSelf);

        Debug.Log($"All objects from pool with tag {poolTag} is used, pool was expanded");
        pool.SizeOfPool++;

        return CreatePoolObject(pool);
    }

    private bool CheckIfPoolCreated(PoolID poolTag)
    {
        if (_poolsDictionary.ContainsKey(poolTag))
            return true;

        Debug.LogError($"There no pool with that tag {poolTag.ToString()} in pools dictionary");
        return false;
    }

    private Pool FindGameObjectInPool(ServiceLocator gameObjectServiceLocator) =>
        _pools.Find(pool => pool.ListOfPool.Contains(gameObjectServiceLocator));
}
