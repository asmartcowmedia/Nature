using UnityEngine;
using Sirenix.OdinInspector;

public class PlateManager2 : MonoBehaviour
{
    [SerializeField] private PlatePuzzle[] plates;
    [SerializeField] private GameObject doorToOpen;

    [ReadOnly][SerializeField] private bool isActive;

    private void Update()
    {
        CheckPlates();
    }

    private void OpenDoors()
    {
        doorToOpen.SetActive(false);
    }

    private void CheckPlates()
    {
        foreach (var t in plates)
        {
            if (!t.isActive)
                return;

            isActive = true;
        }

        if (isActive)
            OpenDoors();
    }
}