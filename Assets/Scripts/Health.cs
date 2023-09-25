using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour, IServiceLocatorComponent, IDamagable, IStartable
{
    public ServiceLocator MyServiceLocator { get; set; }

    public event Action<ServiceLocator> OnHealthTaken;
    public event Action<ServiceLocator> OnDead;

    [SerializeField] private int _startHealth;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private int _health;

    public void TakeDamage()
    {
        _health--;
        textMeshProUGUI.text = _health.ToString();

        if(_health <= 0 ) OnDead?.Invoke(MyServiceLocator);
        else OnHealthTaken?.Invoke(MyServiceLocator);

    }

    public void CustomStart()
    {
        _health = _startHealth;
    }
}

public interface IDamagable
{
    public event Action<ServiceLocator> OnHealthTaken;
    public event Action<ServiceLocator> OnDead;

    public void TakeDamage();
}

