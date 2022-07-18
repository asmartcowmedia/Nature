using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Title("Menu Buttons")][SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
            continueGameButton.interactable = false;
    }

    public void OnNewGameClicked()
    {
        DisableMenuButtons();
        DataPersistenceManager.Instance.NewGame();

        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        SceneManager.LoadSceneAsync("Tutorial");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}