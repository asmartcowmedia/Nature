using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Damager : MonoBehaviour
{
    [SerializeField] private float 
        damageAmount,
        intervalTimer;

    [SerializeField] private bool intervalDamage;

    private HP playerHp;

    private bool interval;

    private Vector3 playerLocation;

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
        if (other.CompareTag("Player"))
        {
            var go = other.gameObject;
            playerHp = go.GetComponent<HP>();
            go.GetComponent<CharacterController>().hurtDirection = transform.position;
            go.GetComponent<CharacterController>().isBeingHurt = true;
        
            interval = intervalDamage;
        
            StartCoroutine(DamageOverTime(intervalTimer));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterController>().isBeingHurt = false;
            interval = false;
        }
    }
}