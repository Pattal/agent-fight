using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour, IServiceLocatorComponent
{
    public ServiceLocator MyServiceLocator { get; set; }
    
    private readonly WaitForSeconds _waitForSecond = new(1);

    public IEnumerator Rotate()
    {
        while (true)
        {
            yield return _waitForSecond;

            var rotationValue = Random.Range(0, 360);
            MyServiceLocator.transform.Rotate(0, rotationValue, 0);
        }
        
    }
}
