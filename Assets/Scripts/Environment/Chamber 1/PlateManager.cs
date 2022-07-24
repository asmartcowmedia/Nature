using UnityEngine;
using Sirenix.OdinInspector;

public class PlateManager : MonoBehaviour
{
    [SerializeField] private Plate[] plates;
    [SerializeField] private GameObject[] doorsToOpen;

    [ReadOnly][SerializeField] private bool isActive;

    private void Update()
    {
        CheckPlates();
    }

    private void OpenDoors()
    {
        foreach (var t in doorsToOpen)
        {
            t.SetActive(false);
        }
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