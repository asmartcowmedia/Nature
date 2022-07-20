using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string sceneToChangeTo;
    [SerializeField] private DataPersistenceManager dataManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        dataManager.SaveGame();

        Debug.Log("Loading next scene: " + sceneToChangeTo);
        SceneManager.LoadSceneAsync(sceneToChangeTo);
    }
}