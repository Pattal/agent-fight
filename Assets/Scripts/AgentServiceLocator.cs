using UnityEngine;

public class AgentServiceLocator : ServiceLocator
{
    protected override void Start()
    {
        base.Start();

        if (TryGetServiceLocatorComponent(out Rotator rotator))
            StartCoroutine(rotator.Rotate());
    }
}

public interface IServiceLocatorComponent 
{
    public ServiceLocator MyServiceLocator { get; set; }
}

public interface IStartable
{
    public void CustomStart();
}

public interface IAwakable
{
    public void CustomAwake();
}

public interface IManager
{
    public void Reset();
}

public interface ISpawnableOnMap
{
    public bool CoordiantesAreInsideObject(Vector3 coordiantes);
}

