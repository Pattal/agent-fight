using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameVariantUI : MonoBehaviour
{
    public string Quantinty { set => _quantintyText.text = value; }

    public GameVariant GameVariant;
    public Toggle Toggle;
    public Action<int> OnToggleSelected;

    [SerializeField] private TextMeshProUGUI _quantintyText;
}

