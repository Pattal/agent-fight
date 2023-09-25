using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServiceLocator : MonoBehaviour, IServiceLocatorComponent
{
    public ServiceLocator MyServiceLocator { get; set; }
    public List<IServiceLocatorComponent> ServiceLocatorComponents => _serviceLocatorComponents;

    private List<IServiceLocatorComponent> _serviceLocatorComponents = new();
    private List<IStartable> _starts = new();
    private List<IAwakable> _awakes = new();

    protected virtual void Awake()
    {
        GetServiceLocatorComponents();
        
        Utilities.TryCast(_serviceLocatorComponents, _starts);
        Utilities.TryCast(_serviceLocatorComponents, _awakes);

        InjectServiceLocatorToComponents();

        AwakeComponents();
    }

    protected virtual void Start()
    {
        StartComponents();
    }

    protected virtual void AwakeComponents()
    {
        _awakes.ForEach(awake => awake.CustomAwake());
    }

    protected virtual void StartComponents()
    {
        _starts.ForEach(start => start.CustomStart());
    }

    public bool TryGetServiceLocatorComponent<T>(out T component)
    {
        foreach (IServiceLocatorComponent serviceLocatorComponent in _serviceLocatorComponents)
        {
            if (serviceLocatorComponent is T)
            {
                component = (T)serviceLocatorComponent;
                return true;
            }
        }

        component = default;
        Debug.LogError($"Searched component {typeof(T)} was not found", gameObject);
        return false;
    }


    private void InjectServiceLocatorToComponents()
    {
        _serviceLocatorComponents.ForEach(component => component.MyServiceLocator = this);
    }

    private void GetServiceLocatorComponents()
    {
        _serviceLocatorComponents = new();
        _serviceLocatorComponents = GetComponents<IServiceLocatorComponent>().ToList();

        _serviceLocatorComponents.AddRange(GetComponentsInChildren<IServiceLocatorComponent>().ToList());
    }

}
