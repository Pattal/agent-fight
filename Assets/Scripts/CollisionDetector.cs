using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour, IServiceLocatorComponent
{
    public ServiceLocator MyServiceLocator { get; set; }

    [SerializeField] private ObejctPoolerVariable _obejctPooler;

    private void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponentInParent<IDamagable>();

        if (damagable == null) return;

        damagable.TakeDamage();
        _obejctPooler.Value.DisableGameObjectFromPool(MyServiceLocator);
    }
}
