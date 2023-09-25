using UnityEngine;

public class BulletMovement : MonoBehaviour, IServiceLocatorComponent
{
    public ServiceLocator MyServiceLocator { get; set; }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime;
    }
}





