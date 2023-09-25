using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Area Manager", menuName = "ScriptableObjects/Managers/GameAreaManager")]
public class GameAreaManager : ScriptableObject, IManager, IStartable
{
    private List<ISpawnableOnMap> _spawnables = new();

    [SerializeField] ObejctPoolerVariable _pooler;
    [SerializeField] MeshColliderVariable _meshCollider;
    [SerializeField] PoolID _objectToSpawn;

    private Vector3 _boundsMin;
    private Vector3 _boundsMax;

    public void CustomStart()
    {
        _pooler.Value.InitializePools();

        GetBoundsSize();

        SpawnObjects(7);
    }

    public bool IsPointInBounds(Vector3 point)
    {
        return point.x > _boundsMin.x && point.x < _boundsMax.x && point.z > _boundsMin.z && point.z < _boundsMax.z;
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
        if(!TryGetEmptySpaceOnMap(boundsMax, boundsMin, out Vector3 startingPos)) 
            return false;

        ServiceLocator spawnedObject = _pooler.Value.SpawnFromPool(_objectToSpawn, startingPos, Quaternion.identity, _pooler.Value.transform);

        if (!spawnedObject.TryGetServiceLocatorComponent(out ISpawnableOnMap spawnable))
        {
            _pooler.Value.DisableGameObjectFromPool(spawnedObject);
            return false;
        }

        _spawnables.Add(spawnable);
        return true;
    }


    private bool TryGetEmptySpaceOnMap(Vector3 boundsMax, Vector3 boundsMin, out Vector3 newPosition)
    {
        int numberOfExecution = 0;

        while (numberOfExecution < 200)
        {
            newPosition = new Vector3(Random.Range(boundsMin.x, boundsMax.x), 0, Random.Range(boundsMin.x, boundsMax.z));
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
            if (spawnedObject.CoordiantesAreInsideObject(newPosition))
            {
                return false;
            }
        }

        return true;
    }

    private void GetBoundsSize()
    {
        _boundsMax = _meshCollider.Value.bounds.max;
        _boundsMin = _meshCollider.Value.bounds.min;
    }

    public void Reset()
    {
        _spawnables.Clear();
    }
}
