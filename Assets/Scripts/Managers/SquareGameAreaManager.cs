using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Square Game Area Manager", menuName = "ScriptableObjects/Managers/SquareGameAreaManager")]
public class SquareGameAreaManager : GameAreaManager<MeshColliderVariable>, IStartable
{
    private Vector3 _boundsMin;
    private Vector3 _boundsMax;

    public void CustomStart()
    {
        GetBoundsSize();
    }

    public override Vector3 GetRandomPointInBounds() => new (Random.Range(_boundsMin.x, _boundsMax.x), 0, Random.Range(_boundsMin.x, _boundsMax.z));

    public override bool IsPointInBounds(Vector3 point) => point.x > _boundsMin.x && point.x < _boundsMax.x && point.z > _boundsMin.z && point.z < _boundsMax.z;
  

    private void GetBoundsSize()
    {
        _boundsMax = _collider.Value.bounds.max;
        _boundsMin = _collider.Value.bounds.min;
    }
}
