using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    [Title("Menu Navigation")][SerializeField] private MainMenu mainMenu;
    
    [Title("Menu Buttons")][SerializeField] private Button backButton;
    
    private GameData gameData;
    
    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        
        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        
        if (!isLoadingGame) 
            DataPersistenceManager.Instance.NewGame();

        if (DataPersistenceManager.Instance.gameData.currentScene == "")
        {
            Debug.Log("scene has no name... loading default");
            SceneManager.LoadSceneAsync("Tutorial");
        }
        else
        {
            Debug.Log("scene is loading " + DataPersistenceManager.Instance.GetSavedSceneName());
            SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.GetSavedSceneName());
        }
        
        DataPersistenceManager.Instance.SaveGame();
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void ActivateMenu(bool isLoading)
    {
        gameObject.SetActive(true);

        isLoadingGame = isLoading;
        
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        var firstSelected = backButton.gameObject;
        foreach (var saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            
            if (profileData == null && isLoadingGame)
                saveSlot.SetInteractable(false);
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                    firstSelected = saveSlot.gameObject;
            }
        }

        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (var saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }

        backButton.interactable = false;
    }
}