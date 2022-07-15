using System.Collections;
using System.Globalization;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class HP : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject player;

    [SerializeField] private SpriteRenderer sprite;
    
    [SerializeField] private float maxHealth;
    
    [ShowInInspector][ReadOnly] private float health;

    [SerializeField] private GameObject healthBar;

    [SerializeField] private Slider healthBarFill;
    
    [SerializeField] private TextMeshProUGUI healthBarText;

    [SerializeField] private Color damageColor;

    [SerializeField] private float damageGrowMultiplier;

    private bool visualFeedback;

    private Vector3 defaultSize;

    private CharacterController controller;


    public void LoadData(GameData data)
    {
        health = data.health;
    }

    public void SaveData(ref GameData data)
    {
        data.health = health;
    }
    
    private IEnumerator VisualFeedback()
    {
        while (visualFeedback)
        {
            SetColor(damageColor);
            ChangeSize();

            yield return new WaitForSeconds(.5f);
            
            ResetSize();
            ResetColor();
            
            yield return new WaitForSeconds(.2f);
        }
    }

    private IEnumerator Kill()
    {
        SetColor(damageColor);
        ChangeSize();
        
        yield return new WaitForSeconds(2f);
        
        Destroy(gameObject);
    }

    private void Awake()
    {
        if (healthBar == null || healthBarFill == null || healthBarText == null) Debug.Log("Please set up the health bar for it to work!");
        
        defaultSize = player.GetComponent<CharacterController>().graphicsScale;
        controller = player.GetComponent<CharacterController>();
        
        health = maxHealth;
    }

    private void Update()
    {
        UpdateHUD();
        
        if (health == 0)
            Die();
    }

    private void LateUpdate()
    {
        visualFeedback = false;
    }

    private void UpdateHUD()
    {
        healthBarFill.value = health;
        healthBarFill.maxValue = maxHealth;
        healthBarFill.minValue = 0;

        healthBarText.text = health.ToString(CultureInfo.InvariantCulture);
    }

    private void Die()
    {
        player.SetActive(false);
    }

    public void Damage(float damage)
    {
        health -= damage;

        if (health < 0)
            health = 0;
        
        visualFeedback = true;

        StartCoroutine(VisualFeedback());
    }

    public void Heal(float heal)
    {
        health += heal;

        if (health > maxHealth)
            health = maxHealth;
    }
    private void SetColor(Color color)
    {
        sprite.color = color;
    }

    private void ResetColor()
    {
        sprite.color = Color.white;
    }

    private void ChangeSize()
    {
        var size = Vector3.Lerp(defaultSize, defaultSize * damageGrowMultiplier, 1);
        controller.graphicsScale = size;
    }

    private void ResetSize()
    {
        var size = Vector3.Lerp(defaultSize * damageGrowMultiplier, defaultSize, 1);
        controller.graphicsScale = size;
    }
}