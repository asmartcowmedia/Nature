using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

namespace CampingTrip
{
    public class MenuManager : MonoBehaviour
    {
        // Variables //
        [FoldoutGroup("Variables")]
        [Title("Menu Objects", TitleAlignment = TitleAlignments.Centered)][SerializeField] public GameObject pauseMenu;

        // Private variables //
        private PlayerControls inputSystem;
        private InputAction pause,
            enter;
        
        // Default Unity functions //
        private void Awake()
        {
            inputSystem = new PlayerControls(); // Instantiates the "inputSystem" Variable with the actual player controls scheme
        }

        private void Update()
        {
            PauseMenuUpdate();
        }

        private void OnEnable() // Called when the object is active in the scene
        {
            // Set all the variables for the input system to that of their counterparts in the input scheme
            pause = inputSystem.UI.Pause;
            enter = inputSystem.UI.Submit;
        
            // Enable all input systems when activating object
            pause.Enable();
            enter.Enable();
        }

        private void OnDisable() // Called when the object is disabled in the scene
        {
            // Disable input system on disabling the object
            pause.Disable();
            enter.Disable();
        }
        
        // Private functions //
        private void PauseMenuUpdate()
        {
            if (pause.WasPressedThisFrame())
            {
                FindObjectOfType<Inventory>().inventory.Save();
            }

            if (enter.WasPressedThisFrame())
            {
                FindObjectOfType<Inventory>().inventory.Load();
            }
        }
    }
}