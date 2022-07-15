using System.Collections;
using System.Globalization;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
      //----------------------------------------//
     // Exposed Variables (Editable in editor) //
    //----------------------------------------//
    [FoldoutGroup("Attachable Objects")][Title("GameObjects")][SerializeField] private GameObject staminaBar;
    
    [FoldoutGroup("Attachable Objects")][Title("Sliders")][SerializeField] private Slider staminaBarFill;
    
    [FoldoutGroup("Attachable Objects")][Title("TextMeshProUGUI")][SerializeField] private TextMeshProUGUI staminaBarText;
    
    [FoldoutGroup("Variables")][Title("Stamina")] [SerializeField] private float maxStamina;
    [FoldoutGroup("Variables")][SerializeField] private float staminaRegenAmount;
    [FoldoutGroup("Variables")][ShowInInspector][ReadOnly] public float stamina;
    //----------------------------------------//
    
      //------------------------------------------------//
     // Non-Exposed Variables (Not Editable in editor) //
    //------------------------------------------------//
    private readonly WaitForSeconds regenTick = new WaitForSeconds(.2f);
    private Coroutine regen;
    //----------------------------------------//
    
      //-----------------------//
     // IEnumerator Functions //
    //-----------------------//
    private IEnumerator RegenStamina()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(2);

        //As long as the stamina is lower than max, regen stamina over time of ticks
        while (stamina < maxStamina)
        {
            stamina += staminaRegenAmount;

            staminaBarFill.value = stamina;
            
            yield return regenTick;
        }

        //Reset regen
        regen = null;
    }
    //----------------------------------------//

      //-------------------------//
     // Default Unity Functions //
    //-------------------------//
    private void Awake()
    {
        if (staminaBar == null || staminaBarFill == null || staminaBarText == null) Debug.Log("Please set up the stamina bar for it to work!");
        
        stamina = maxStamina;
    }

    private void Update()
    {
        UpdateHUD();
    }
    //----------------------------------------//

    //------------------//
    // Custom Functions //
    //------------------//
    //Function to update the HUD with stamina information
    private void UpdateHUD()
    {
        staminaBarFill.value = stamina;
        staminaBarFill.maxValue = maxStamina;
        staminaBarFill.minValue = 0;

        staminaBarText.text = stamina.ToString(CultureInfo.InvariantCulture);
    }

    //Function to drain stamina and start the regen ienumerator function
    public void DrainStamina(float amount)
    {
        if (stamina - amount >= 0)
        {
            stamina -= amount;
            staminaBarFill.value = stamina;

            if (regen != null)
                StopCoroutine(regen);
            
            StartCoroutine(RegenStamina());
        }
    }
}