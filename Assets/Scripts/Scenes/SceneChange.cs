using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string sceneToChangeTo;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Loading next scene: " + sceneToChangeTo);
        SceneManager.LoadSceneAsync(sceneToChangeTo);
    }
}