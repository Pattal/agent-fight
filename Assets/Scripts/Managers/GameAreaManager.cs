using System;
using UnityEngine;

public abstract class GameAreaManager<T> : ScriptableObject, IManager where T : UnityEngine.Object
{
    public Bootstrapper Bootstrapper { get; set; }

    [SerializeField] protected T _collider;
    [SerializeField] private ObjectLifecycleManager _objectsLifecycleManager;
   
    public abstract bool IsPointInBounds(Vector3 point);

    public abstract Vector3 GetRandomPointInBounds();

    public bool TryGetEmptySpaceOnMap(out Vector3 newPosition) //simple recursion method that using random point on game area. It could be also done by sampling game area.
    {
        int numberOfExecution = 0;

        while (numberOfExecution < 200)
        {
            newPosition = GetRandomPointInBounds();
            if (!IsPlaceEmpty(newPosition))
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
        foreach (ISpawnableOnMap spawnedObject in _objectsLifecycleManager.GetCurrentlySpawnedObjects())
        {
            if (spawnedObject.IsPlaceTakenByObject(newPosition))
            {
                return false;
            }
        }

        return true;
    }

    public void Reset()
    {  
    }
}
