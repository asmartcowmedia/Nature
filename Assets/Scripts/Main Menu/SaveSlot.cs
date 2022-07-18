using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Title("Profile")][SerializeField] private string profileId;

    [Title("Content")][SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI 
        percentComplete,
        collectablesCollected;

    private Button saveSlotsButton;

    private void Awake()
    {
        saveSlotsButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            percentComplete.text = data.GetPercentageComplete() + "% COMPLETE";
            collectablesCollected.text = "COLLECTABLES COLLECTED: " + data.collectablesCollected;
        }
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotsButton.interactable = interactable;
    }
}