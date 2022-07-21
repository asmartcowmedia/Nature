using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Title("Debugging")][SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool disablePersistence = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId;

    [Header("File Storage Config")][SerializeField] private string fileName;
    [SerializeField] private string pathName;

    [SerializeField] private bool useEncryption;

    [SerializeField] private bool resetSave;
    
    public GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    private string 
        selectedProfileId,
        currentScene;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene! Destroying new one, keeping old!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (disablePersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }
        
        dataHandler = new FileDataHandler(pathName, fileName, useEncryption);

        selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        
        if (overrideSelectedProfileId)
        {
            selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with: " + testSelectedProfileId);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        gameData.currentScene = SceneManager.GetActiveScene().name;
        
        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        selectedProfileId = newProfileId;
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        if (disablePersistence)
            return;
        
        gameData = dataHandler.Load(selectedProfileId);
        
        if (gameData == null && initializeDataIfNull)
            NewGame();
        
        if (gameData == null || resetSave)
        {
            Debug.Log("No Data was found, A new game needs to be started before data can be loaded.");
            return;
        }
        
        if(gameData.health == 0)
            NewGame();

        foreach (var dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disablePersistence)
            return;
        
        if (gameData == null)
        {
            Debug.Log("No Data was found, A new game needs to be started before data can be loaded.");
            return;
        }
        
        foreach (var dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        var scene = SceneManager.GetActiveScene();
        
        if (!scene.name.Equals("Main Menu"))
        {
            Debug.Log("saving scene " + scene.name);
            gameData.currentScene = scene.name;
        }

        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> persistenceObjects =
            FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(persistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public string GetSavedSceneName()
    {
        if (gameData == null)
        {
            Debug.LogError("Tried to get scene name but data was null.");
            return null;
        }

        return gameData.currentScene;
    }
}