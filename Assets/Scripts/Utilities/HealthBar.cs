using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private Image barImageYellow;

    public void UpdateHealthBar(float maxHealth, float health)
    {
        barImage.fillAmount = health / maxHealth;
        StartCoroutine(ChangeHit(maxHealth, health));
    }

    IEnumerator ChangeHit(float maxHealth1, float health1)
    {
        yield return new WaitForSeconds(0.3f);
        barImageYellow.fillAmount = health1 / maxHealth1;
    }
}
