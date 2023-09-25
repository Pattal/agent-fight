using System.Collections;
using UnityEngine;

public abstract class CoroutineBasedServiceLocator : MonoBehaviour, IServiceLocatorComponent
{
    public ServiceLocator MyServiceLocator { get; set; }

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _coroutine = StartCoroutine(Coroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    protected abstract IEnumerator Coroutine();

}

