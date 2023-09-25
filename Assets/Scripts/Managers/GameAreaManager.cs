using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Game Area Manager", menuName = "ScriptableObjects/Managers/GameAreaManager")]
public class GameAreaManager : ScriptableObject, IManager, IStartable
{
    public Bootstrapper Bootstrapper { get; set; }
    public event Action OnLastAgentExist;

    private List<ISpawnableOnMap> _spawnables = new();

    [SerializeField] ObejctPoolerVariable _pooler;
    [SerializeField] MeshColliderVariable _meshCollider;
    [SerializeField] PoolID _agentToSpawn;
    [SerializeField] PoolID _bulletToSpawn;

    private Vector3 _boundsMin;
    private Vector3 _boundsMax;

    private WaitForSeconds _waitTwoSeconds = new(2);

    public void CustomStart()
    {
        _pooler.Value.InitializePools();
        GetBoundsSize();
    }

    public bool IsPointInBounds(Vector3 point)
    {
        return point.x > _boundsMin.x && point.x < _boundsMax.x && point.z > _boundsMin.z && point.z < _boundsMax.z;
    }

    public void Spawn50()
    {
        SpawnObjects(5);
    }

    private void SpawnObjects(int quantity)
    {
        
        for (int i = 0; i < quantity; i++)
        {
            if(!TrySpawnObject(_boundsMax, _boundsMin)) continue;
        }

    }

    private bool TrySpawnObject(Vector3 boundsMax, Vector3 boundsMin)
    {
        if(!TryGetEmptySpaceOnMap(out Vector3 startingPos)) 
            return false;

        ServiceLocator spawnedObject = _pooler.Value.SpawnFromPool(_agentToSpawn, startingPos, Quaternion.identity, _pooler.Value.transform);

        if (!spawnedObject.TryGetServiceLocatorComponent(out ISpawnableOnMap spawnable) ||
            !spawnedObject.TryGetServiceLocatorComponent(out IDamagable damagable))
        {
            _pooler.Value.DisableGameObjectFromPool(spawnedObject);
            return false;
        }

        damagable.OnHealthTaken += StartRespawnCoroutine;
        damagable.OnDead += StartKillCoroutine;

        _spawnables.Add(spawnable);

        return true;
    }


    public bool TryGetEmptySpaceOnMap(out Vector3 newPosition)
    {
        int numberOfExecution = 0;

        while (numberOfExecution < 200)
        {
            newPosition = new Vector3(Random.Range(_boundsMin.x, _boundsMax.x), 0, Random.Range(_boundsMin.x, _boundsMax.z));
            if(!IsPlaceEmpty(newPosition))
            {
                numberOfExecution++;
                continue;
            }

            return true;
        }

        newPosition = default;
        return false;
    }

    private bool IsPlaceEmpty(Vector3 newPosition)
    {
        foreach (ISpawnableOnMap spawnedObject in _spawnables)
        {
            if (spawnedObject.IsPlaceTakenByObject(newPosition))
            {
                return false;
            }
        }

        return true;
    }

    private void StartRespawnCoroutine(ServiceLocator objectToRespawn)
    {
        Bootstrapper.StartCoroutine(Respawn(objectToRespawn));
    }

    private IEnumerator Respawn(ServiceLocator objectToRespawn)
    {
        objectToRespawn.gameObject.SetActive(false);
        yield return _waitTwoSeconds;

        if (!TryGetEmptySpaceOnMap(out Vector3 newPosition))
            yield return Bootstrapper.StartCoroutine(Respawn(objectToRespawn));

        objectToRespawn.transform.position = newPosition;
        objectToRespawn.gameObject.SetActive(true);

    }


    private void StartKillCoroutine(ServiceLocator objectToKill)
    {
        Bootstrapper.StartCoroutine(Kill(objectToKill));
    }

    private IEnumerator Kill(ServiceLocator objectToKill)
    {
        _pooler.Value.DisableGameObjectFromPool(objectToKill);

        objectToKill.TryGetServiceLocatorComponent(out Health health);
        objectToKill.TryGetServiceLocatorComponent(out ISpawnableOnMap spawnable);

        health.OnHealthTaken -= StartRespawnCoroutine;
        health.OnDead -= StartKillCoroutine;

        health.ResetHealth();
        _spawnables.Remove(spawnable);

        if (_spawnables.Count < 2 ) 
        {
            OnLastAgentExist?.Invoke();
        }

        yield return null;
    }

    private void GetBoundsSize()
    {
        _boundsMax = _meshCollider.Value.bounds.max;
        _boundsMin = _meshCollider.Value.bounds.min;
    }

    public void Reset()
    {
        _spawnables.Clear();
        _pooler.Value.DisableAllActiveObjectsInPool(_agentToSpawn);
        _pooler.Value.DisableAllActiveObjectsInPool(_bulletToSpawn);
    }
}

