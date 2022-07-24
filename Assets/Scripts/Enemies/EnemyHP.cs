using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private EnemyAI enemy;

    [SerializeField] private float 
        takeDamageAmount,
        maxHealth,
        damageGrowMultiplier;

    [SerializeField] private Color damageColor;
    
    [ShowInInspector][ReadOnly] public float health;

    private bool visualFeedback;

    private Vector3 defaultSize;
    
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

    private void Start()
    {
        health = maxHealth;
        defaultSize = enemy.graphicsScale;
    }

    private void Update()
    {
        if (health <= 0)
            Die();
    }

    private void LateUpdate()
    {
        visualFeedback = false;
    }

    private void Die()
    {
        StartCoroutine(Kill());
    }

    private void Damage(float damage)
    {
        health -= damage;

        if (health < 0)
            health = 0;
        
        visualFeedback = true;

        StartCoroutine(VisualFeedback());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerAttack"))
        {
            Damage(takeDamageAmount);
        }
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
        enemy.graphicsScale = size;
    }

    private void ResetSize()
    {
        var size = Vector3.Lerp(defaultSize * damageGrowMultiplier, defaultSize, 1);
        enemy.graphicsScale = size;
    }
}