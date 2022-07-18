using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Title("First Selected Button")][SerializeField] private Button firstSelected;

    protected virtual void OnEnable()
    {
        SetFirstSelected(firstSelected);
    }

    public void SetFirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}