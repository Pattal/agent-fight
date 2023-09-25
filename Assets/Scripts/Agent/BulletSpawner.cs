using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : CoroutineBasedServiceLocator
{
    [SerializeField] private ObejctPoolerVariable _objectPooler;
    [SerializeField] private PoolID _objectToSpawn;
    [SerializeField] private Transform _gun;

    private readonly WaitForSeconds _waitForSecond = new(1);

    protected override IEnumerator Coroutine()
    {
        while (true)
        {
            yield return _waitForSecond;

            _objectPooler.Value.SpawnFromPool(_objectToSpawn, _gun.position, MyServiceLocator.transform.rotation, _objectPooler.Value.transform);
        }
    }
}
