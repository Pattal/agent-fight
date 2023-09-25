using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour, IServiceLocatorComponent
{
    public ServiceLocator MyServiceLocator { get; set; }

    [SerializeField] ObejctPoolerVariable _objectPooler;
    [SerializeField] PoolID _objectToSpawn;
    [SerializeField] Transform _bulletsHolder;
    [SerializeField] Transform _gun;

    private Coroutine _spawnBulletCoroutine;
    private readonly WaitForSeconds _waitForSecond = new(1);

    private void OnEnable()
    {
        _spawnBulletCoroutine = StartCoroutine(SpawnBullet());
    }

    private void OnDisable()
    {
        StopCoroutine(_spawnBulletCoroutine);
    }

    private IEnumerator SpawnBullet()
    {
        while(true)
        {
            yield return _waitForSecond;
            _objectPooler.Value.SpawnFromPool(_objectToSpawn, _gun.position, MyServiceLocator.transform.rotation, _objectPooler.Value.transform);
        }
        
    }
}
