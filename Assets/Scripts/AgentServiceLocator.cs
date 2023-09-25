using UnityEngine;

public class AgentServiceLocator : ServiceLocator
{
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
    public Bootstrapper Bootstrapper { get; set; }
    public void Reset();
}

public interface ISpawnableOnMap
{
    public bool IsPlaceTakenByObject(Vector3 coordiantes);
}

