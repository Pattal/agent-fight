using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour, IServiceLocatorComponent, IDamagable, IStartable, IReset
{
    public ServiceLocator MyServiceLocator { get; set; }

    public event Action<ServiceLocator> OnHealthTaken;
    public event Action<ServiceLocator> OnDead;

    [SerializeField] private int _startHealth;

    private int _health;

    public void CustomStart()
    {
        SetStartingHealth();
    }

    public void TakeDamage()
    {
        _health--;

        if(_health <= 0 ) 
            OnDead?.Invoke(MyServiceLocator);
        else 
            OnHealthTaken?.Invoke(MyServiceLocator);
    }

    private void SetStartingHealth()
    {
        _health = _startHealth;
    }

    public void Reset()
    {
        SetStartingHealth();
    }
}



