using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects Lifecycle Manager", menuName = "ScriptableObjects/ObjectsLifecycleManager")]
public class ObjectLifecycleManager : ScriptableObject, IManager, IStartable
{
    public Bootstrapper Bootstrapper { get; set; }
    public event Action OnLastAgentExist;

    [SerializeField] private AgentSpawner _agentSpawner;
    [SerializeField] private ObejctPoolerVariable _poolerVariable;
    [SerializeField] private SquareGameAreaManager _squareGameAreaManager;

    private List<ISpawnableOnMap> _spawnedAgents = new();

    private readonly WaitForSeconds _waitTwoSeconds = new(2);

    public void CustomStart()
    {
        _agentSpawner.OnAgentSpawn += AddAgentToList;
    }

    public List<ISpawnableOnMap> GetCurrentlySpawnedObjects() => _spawnedAgents;

    private void StartRespawnCoroutine(ServiceLocator objectToRespawn)
    {
        Bootstrapper.StartCoroutine(Respawn(objectToRespawn));
    }

    private IEnumerator Respawn(ServiceLocator objectToRespawn)
    {
        objectToRespawn.gameObject.SetActive(false);
        yield return _waitTwoSeconds;

        if (!_squareGameAreaManager.TryGetEmptySpaceOnMap(out Vector3 newPosition))
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
        _poolerVariable.Value.DisableGameObjectFromPool(objectToKill);

        objectToKill.TryGetServiceLocatorComponent(out Health health);
        objectToKill.TryGetServiceLocatorComponent(out ISpawnableOnMap spawnable);

        health.OnHealthTaken -= StartRespawnCoroutine;
        health.OnDead -= StartKillCoroutine;

        _spawnedAgents.Remove(spawnable);

        Debug.Log(_spawnedAgents.Count);
        if (_spawnedAgents.Count < 2)
        {
            OnLastAgentExist?.Invoke();
        }

        yield return null;
    }

    private void AddAgentToList(ISpawnableOnMap spawnable, IDamagable damagable)
    {
        _spawnedAgents.Add(spawnable);

        damagable.OnHealthTaken += StartRespawnCoroutine;
        damagable.OnDead += StartKillCoroutine;
    }

    public void Reset()
    {
        _spawnedAgents.Clear();
    }
}