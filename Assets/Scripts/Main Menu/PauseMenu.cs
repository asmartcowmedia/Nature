using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Title("Attachables")][SerializeField] private GameObject pauseMenu;
    
    public static PauseMenu Instance { get; private set; }
    
    private PlayerControls input;
    private InputAction pause;
    private bool paused;

    private void KeepOnDestroy()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one PauseMenu in the scene! Destroying new one, keeping old!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        pause = input.UI.Cancel;
        pause.Enable();
    }

    private void OnDisable()
    {
        pause.Disable();
    }

    private void Awake()
    {
        KeepOnDestroy();
        input = new PlayerControls();
    }

    private void Update()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        if (pause.WasPressedThisFrame())
        {
            if (paused)
            {
                paused = false;
                pauseMenu.SetActive(false);
            }
            else if (!paused)
            {
                paused = true;
                pauseMenu.SetActive(true);
            }
        }
    }

    public void QuitToMainMenu()
    {
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void SaveGame()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void QuitToDesktop()
    {
        DataPersistenceManager.Instance.SaveGame();
        Application.Quit();
    }
}