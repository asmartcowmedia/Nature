using System.Collections;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private float 
        damageAmount,
        intervalTimer;

    [SerializeField] private bool intervalDamage;

    private HP playerHp;

    private bool interval;

    private IEnumerator DamageOverTime(float time)
    {
        while (interval)
        {
            playerHp.Damage(damageAmount);
            
            yield return new WaitForSeconds(time);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerHp = other.gameObject.GetComponent<HP>();
        
        interval = intervalDamage;
        
        StartCoroutine(DamageOverTime(intervalTimer));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        interval = false;
    }
}