using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneEnter : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject player;
    [SerializeField] private VideoPlayer video;

    private IEnumerator VideoEnds()
    {
        yield return new WaitForSeconds(1f);
        while (video.isPlaying)
        {
            yield return null;
        }
        
        ResetCameraAndPlayer();
    }
    
    private void Start() 
    {
        video.playOnAwake = false;
        video.renderMode = VideoRenderMode.CameraNearPlane;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        player.SetActive(false);
        ui.SetActive(false);
        
        video.gameObject.SetActive(true);
        video.Play();
        StartCoroutine(VideoEnds());
    }

    private void ResetCameraAndPlayer()
    {
        video.gameObject.SetActive(false);
        
        player.SetActive(true);
        ui.SetActive(true);
        
        gameObject.SetActive(false);
    }
}