using System.Collections;
using System.Globalization;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    
    [SerializeField] private float maxStamina;

    [SerializeField] private float staminaRegenAmount;

    [ShowInInspector][ReadOnly] public float stamina;

    [SerializeField] private GameObject staminaBar;

    [SerializeField] private Slider staminaBarFill;
    
    [SerializeField] private TextMeshProUGUI staminaBarText;

    private readonly WaitForSeconds regenTick = new WaitForSeconds(.2f);

    private Coroutine regen;

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2);

        while (stamina < maxStamina)
        {
            stamina += staminaRegenAmount;

            staminaBarFill.value = stamina;
            
            yield return regenTick;
        }

        regen = null;
    }

    private void Awake()
    {
        if (staminaBar == null || staminaBarFill == null || staminaBarText == null) Debug.Log("Please set up the stamina bar for it to work!");
        
        stamina = maxStamina;
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        staminaBarFill.value = stamina;
        staminaBarFill.maxValue = maxStamina;
        staminaBarFill.minValue = 0;

        staminaBarText.text = stamina.ToString(CultureInfo.InvariantCulture);
    }

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