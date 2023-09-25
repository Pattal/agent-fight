using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentsSpawner", menuName = "ScriptableObjects/Spawner")]
public class AgentSpawner : ScriptableObject, IManager, IStartable
{
    public event Action<ISpawnableOnMap, IDamagable> OnAgentSpawn;

    [SerializeField] private ObejctPoolerVariable _pooler;
    [SerializeField] private SquareGameAreaManager _areaManager;
    [SerializeField] private PoolID _agentPoolID;
    [SerializeField] private PoolID _bulletPoolID;

    public Bootstrapper Bootstrapper { get; set; }

    public void CustomStart()
    {
        _pooler.Value.InitializePools();
    }

    public void SpawnObjects(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            if (!TrySpawnObject()) continue;
        }
    }

    private bool TrySpawnObject()
    {
        if (!_areaManager.TryGetEmptySpaceOnMap(out Vector3 startingPos))
            return false;

        ServiceLocator spawnedObject = _pooler.Value.SpawnFromPool(_agentPoolID, startingPos, Quaternion.identity, _pooler.Value.transform);

        if (!spawnedObject.TryGetServiceLocatorComponent(out ISpawnableOnMap spawnable) ||
            !spawnedObject.TryGetServiceLocatorComponent(out IDamagable damagable))
        {
            _pooler.Value.DisableGameObjectFromPool(spawnedObject);
            return false;
        }

        OnAgentSpawn.Invoke(spawnable, damagable);
        return true;
    }

    public void Reset()
    {
        _pooler.Value?.DisableAllActiveObjectsInPool(_agentPoolID);
        _pooler.Value?.DisableAllActiveObjectsInPool(_bulletPoolID);
    }
}
