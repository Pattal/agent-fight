using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSelectorUIController : MonoBehaviour
{
    [SerializeField] private List<ButtonAction> _selectableButtons;
    private ButtonAction _selectedButton;

    [SerializeField] Button _confirmButton;

    private void Start()
    {
        _selectableButtons.ForEach(button => button.Toggle.onValueChanged.AddListener((e) => CheckIfConfirmButtonShouldBeOn(button)));
        _confirmButton.onClick.AddListener(() => 
        { 
            _selectedButton.Action?.Invoke(); 
            gameObject.SetActive(false);
        });
    }

    private void CheckIfConfirmButtonShouldBeOn(ButtonAction buttonAction)
    {
        if (_selectedButton == buttonAction) return;
        
        _selectedButton = buttonAction;
        _confirmButton.interactable = _selectedButton != null;
    }

    private void OnDisable()
    {
        _selectedButton = null;
        _confirmButton.interactable = false;
    }
}

