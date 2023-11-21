using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamegeAcidBoss : MonoBehaviour
{
    public float damage;
    public float resetDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DanoAcido(collision, damage));
        }
    }
    IEnumerator DanoAcido(Collider2D collision, float damage1)
    {
        if (damage > 0)
        {
            collision.GetComponent<Player>().ReceiveDamage(damage1);
        }
        yield return new WaitForSeconds(0.5f);
        damage = 0;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        damage = resetDamage;
    }
}
