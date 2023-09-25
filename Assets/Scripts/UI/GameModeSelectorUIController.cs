using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameModeSelectorUIController : MonoBehaviour
{
    [SerializeField] private List<GameVariantUI> _selectableButtons;
    private GameVariantUI _selectedButton;

    [SerializeField] Button _confirmButton;
    [SerializeField] Transform _container;
    [SerializeField] private AgentSpawner _agentSpawner;

    private void Start()
    {
        _confirmButton.onClick.AddListener(() =>
        {
            _selectedButton.OnToggleSelected?.Invoke(_selectedButton.GameVariant.NumberOfAgents);
            gameObject.SetActive(false);
        });
    }

    public void Initialize(List<GameVariant> gameVariants)
    {
        DeleteExistingGameVariants();
        SpawnGameVariantOptions(gameVariants);
        InjectActionsToEachButton();

        gameObject.SetActive(true);
    }

    private void InjectActionsToEachButton()
    {
        _selectableButtons.ForEach(button => button.Toggle.onValueChanged.AddListener((e) => CheckIfConfirmButtonShouldBeOn(button)));
    }

    private void SpawnGameVariantOptions(List<GameVariant> gameVariants)
    {
        foreach (var variant in gameVariants)
        {
            GameVariantUI gameVariantUI = Instantiate(variant.UIPrefab, _container);

            gameVariantUI.Quantinty = variant.NumberOfAgents.ToString();
            gameVariantUI.OnToggleSelected = _agentSpawner.SpawnObjects;
            gameVariantUI.GameVariant = variant;

            _selectableButtons.Add(gameVariantUI);
        }
    }

    private void DeleteExistingGameVariants()
    {
        _selectableButtons.Clear();
        
        for (int i = _container.childCount - 1; i >= 0; i--)
        {
            Destroy(_container.GetChild(i).gameObject);
        }
    }

    private void CheckIfConfirmButtonShouldBeOn(GameVariantUI buttonAction)
    {
        if (_selectedButton == buttonAction) return;
        
        if(_selectedButton != null) _selectedButton.transform.DOScale(1f, 0.5f);
        _selectedButton = buttonAction;
        _selectedButton.transform.DOScale(1.2f, 0.5f);
        
        _confirmButton.interactable = _selectedButton != null;
    }

    private void OnDisable()
    {
        _selectedButton = null;
        _confirmButton.interactable = false;
    }
}

