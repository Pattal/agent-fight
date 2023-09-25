using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletServiceLocator : ServiceLocator
{
    [SerializeField] private ObejctPoolerVariable _obejctPooler;
    [SerializeField] private GameAreaManager _gameAreaManager;

    private void Update()
    {
        if (!_gameAreaManager.IsPointInBounds(transform.position))
            _obejctPooler.Value.DisableGameObjectFromPool(this);

        transform.position += transform.forward * Time.deltaTime ;
    }
}
