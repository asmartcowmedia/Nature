using System.Globalization;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    
    [ShowInInspector][ReadOnly] private float health;

    [SerializeField] private GameObject healthBar;

    [SerializeField] private Slider healthBarFill;
    
    [SerializeField] private TextMeshProUGUI healthBarText;
    
    private void Awake()
    {
        if (healthBar == null || healthBarFill == null || healthBarText == null) Debug.Log("Please set up the health bar for it to work!");
        
        health = maxHealth;
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        healthBarFill.value = health;
        healthBarFill.maxValue = maxHealth;
        healthBarFill.minValue = 0;

        healthBarText.text = health.ToString(CultureInfo.InvariantCulture);
    }

    public void Damage(float damage)
    {
        health -= damage;

        if (health < 0)
            health = 0;
    }

    public void Heal(float heal)
    {
        health += heal;

        if (health > maxHealth)
            health = maxHealth;
    }
}