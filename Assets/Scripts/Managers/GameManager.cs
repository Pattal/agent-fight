using UnityEngine;

[CreateAssetMenu(fileName = "Game Manager", menuName = "ScriptableObjects/Managers/GameManager")]
public class GameManager : ScriptableObject, IManager, IStartable
{
    public Bootstrapper Bootstrapper { get; set; }

    [SerializeField] GameAreaManager _areaManager;
    [SerializeField] EndWindowUIControllerVariable _endWindowUIController;
    [SerializeField] GameModeSelectorUIControllerVariable _gameModeSelectorUIController;

    public void CustomStart()
    {
        _areaManager.OnLastAgentExist += () => ShowEndMenu();
    }

    public void Reset()
    {
        _areaManager.Reset();
    }

    public void MainMenuButtonClicked()
    {
        _gameModeSelectorUIController.Value.gameObject.SetActive(true);
        Reset();
    }

    private void ShowMainMenu()
    {

    }

    private void ShowEndMenu()
    {
        _endWindowUIController.Value.gameObject.SetActive(true);
    }

}

