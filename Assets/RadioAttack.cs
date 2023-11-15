using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioAttack : MonoBehaviour
{
    public Animator enemigoAnimator;
    [SerializeField] private bool atqNormal = false;  
    [SerializeField] private float radiusAttack;
    public float hitDamage;


    public bool AtqNormal
    {
        get { return atqNormal; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            atqNormal = true;
            enemigoAnimator.SetBool("atqNormal", true); 

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            atqNormal = false;
            enemigoAnimator.SetBool("atqNormal", false); 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
    public void AttackPlayer()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, radiusAttack);

        foreach (Collider2D collision in objetos)
        {
            if (collision.gameObject.tag == "Player")
            {
                float playerLife = collision.GetComponent<Player>().health;
                if (playerLife > 4f)
                {
                    collision.GetComponent<Player>().ReceiveDamage(hitDamage);
                }

            }
        }

    }
}

