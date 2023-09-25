using UnityEngine;

public class BulletMovement : MonoBehaviour, IServiceLocatorComponent
{
    public ServiceLocator MyServiceLocator { get; set; }

    [SerializeField] private float _bulletSpeed = 1.0f;

    private void Update()
    {
        transform.position += _bulletSpeed * Time.deltaTime * transform.forward;
    }
}





