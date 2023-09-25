using UnityEngine;

public class CapsuleShapedObject : MonoBehaviour, ISpawnableOnMap, IServiceLocatorComponent
{
    public ServiceLocator MyServiceLocator { get; set; }

    [SerializeField] private CapsuleCollider _capsuleCollider;

    public bool IsPlaceTakenByObject(Vector3 coordiantes)
    {
        var radius = _capsuleCollider.radius;
        return 
            Vector3.Distance(coordiantes, MyServiceLocator.transform.position) < radius * 2 // 2 times because both objects must have space between them
            && MyServiceLocator.gameObject.activeInHierarchy; 
    }
}
