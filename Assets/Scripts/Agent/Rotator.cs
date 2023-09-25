using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : CoroutineBasedServiceLocator
{
    [SerializeField] private float _minRotationTime;
    [SerializeField] private float _maxRotationTime;

    [SerializeField] private float _minRotationAngle;
    [SerializeField] private float _maxRotationAngle;

    protected override IEnumerator Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minRotationTime, _maxRotationTime));

            var rotationValue = Random.Range(_minRotationAngle, _maxRotationAngle);
            MyServiceLocator.transform.Rotate(0, rotationValue, 0);
        }
    }
}

