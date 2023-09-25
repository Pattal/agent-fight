using UnityEngine;

[CreateAssetMenu(fileName = "GameVariant", menuName = "ScriptableObjects/GameVariant")]
public class GameVariant : ScriptableObject
{ 
    [field: SerializeField] public int NumberOfAgents { get; private set; }
    [field: SerializeField] public GameVariantUI UIPrefab { get; private set; }
}


