using UnityEngine;

public class BulletRangeController : MonoBehaviour, IServiceLocatorComponent
{
    [SerializeField] private ObejctPoolerVariable _obejctPooler;
    [SerializeField] private SquareGameAreaManager _gameAreaManager;

    public ServiceLocator MyServiceLocator { get; set; }

    private void Update()
    {
        if (!_gameAreaManager.IsPointInBounds(transform.position))
            _obejctPooler.Value.DisableGameObjectFromPool(MyServiceLocator);
    }
}





