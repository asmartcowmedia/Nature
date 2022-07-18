using System.Collections;
using System.Globalization;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class HP : MonoBehaviour, IDataPersistence
{
      //----------------------------------------//
     // Exposed Variables (Editable in editor) //
    //----------------------------------------//
    [FoldoutGroup("Attachable Objects")][Title("GameObjects")][SerializeField] private GameObject player;
    [FoldoutGroup("Attachable Objects")][SerializeField] private GameObject healthBar;
    
    [FoldoutGroup("Attachable Objects")][Title("Sliders")][SerializeField] private Slider healthBarFill;
    
    [FoldoutGroup("Attachable Objects")][Title("TextMeshProUGUI")][SerializeField] private TextMeshProUGUI healthBarText;

    [FoldoutGroup("Attachable Objects")][Title("Sprite Renderers")][SerializeField] private SpriteRenderer sprite;
    
    [FoldoutGroup("Variables")][Title("Health")][SerializeField] private float maxHealth;
    [FoldoutGroup("Variables")][ShowInInspector][ReadOnly] private float health;
    [FoldoutGroup("Variables")][SerializeField] private Color damageColor;
    
    [FoldoutGroup("Variables")][Title("Graphics")][SerializeField] private float damageGrowMultiplier;
    //----------------------------------------//
    
      //------------------------------------------------//
     // Non-Exposed Variables (Not Editable in editor) //
    //------------------------------------------------//
    private bool visualFeedback;
    private Vector3 defaultSize;
    private CharacterController controller;
    //----------------------------------------//
    
      //--------------------------//
     // Save/Load Data Functions //
    //--------------------------//
    public void LoadData(GameData data)
    {
        health = data.health;
    }

    public void SaveData(GameData data)
    {
        data.health = health;
    }
    //----------------------------------------//
    
      //--------------//
     // IEnumerators //
    //--------------//
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
    //----------------------------------------//

      //-------------------------//
     // Default Unity Functions //
    //-------------------------//
    private void Awake()
    {
        //Checking attachables and calling a log message if not properly set
        if (healthBar == null || healthBarFill == null || healthBarText == null) Debug.Log("Please set up the health bar for it to work!");
        
        //Setting variables initial states
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
        //Resetting visual feedback loop
        visualFeedback = false;
    }
    //----------------------------------------//

      //------------------//
     // Custom Functions //
    //------------------//
    //Function to update the hud with proper values of health
    private void UpdateHUD()
    {
        healthBarFill.value = health;
        healthBarFill.maxValue = maxHealth;
        healthBarFill.minValue = 0;

        healthBarText.text = health.ToString(CultureInfo.InvariantCulture);
    }

    //function that sets the character player to inactive (does not destroy player)
    private void Die()
    {
        player.SetActive(false);
    }

    //Function to damage the player with visual feedback loop
    public void Damage(float damage)
    {
        health -= damage;

        if (health < 0)
            health = 0;
        
        visualFeedback = true;

        StartCoroutine(VisualFeedback());
    }

    //Function to heal player when called
    public void Heal(float heal)
    {
        health += heal;

        if (health > maxHealth)
            health = maxHealth;
    }
    
    //Function to set the color of the selected sprite renderer
    private void SetColor(Color color)
    {
        sprite.color = color;
    }

    //Function to reset the color of the selected sprite renderer
    private void ResetColor()
    {
        sprite.color = Color.white;
    }

    //Function to change the size of the character for visual feedback
    private void ChangeSize()
    {
        var size = Vector3.Lerp(defaultSize, defaultSize * damageGrowMultiplier, 1);
        controller.graphicsScale = size;
    }

    //Function to reset the size change performed in above function
    private void ResetSize()
    {
        var size = Vector3.Lerp(defaultSize * damageGrowMultiplier, defaultSize, 1);
        controller.graphicsScale = size;
    }
    //----------------------------------------//
}