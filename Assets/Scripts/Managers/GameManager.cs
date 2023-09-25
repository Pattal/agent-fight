using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Manager", menuName = "ScriptableObjects/Managers/GameManager")]
public class GameManager : ScriptableObject, IManager, IStartable
{
    public Bootstrapper Bootstrapper { get; set; }

    [SerializeField] private List<GameVariant> _gameVariants;
    [SerializeField] ObjectLifecycleManager _objectLifecycleManager;
    [SerializeField] EndWindowUIControllerVariable _endWindowUIController;
    [SerializeField] GameModeSelectorUIControllerVariable _gameModeSelectorUIController;

    public void CustomStart()
    {
        _objectLifecycleManager.OnLastAgentExist += ShowEndMenu;
        ShowMainMenu();
    }

    public void Reset()
    {
        _objectLifecycleManager.Reset();
    }

    public void MainMenuButtonClicked()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        _gameModeSelectorUIController.Value.Initialize(_gameVariants);
    }

    private void ShowEndMenu()
    {
        _endWindowUIController.Value.gameObject.SetActive(true);
        Bootstrapper.Reset();
    }


}


