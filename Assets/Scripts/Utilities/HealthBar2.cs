using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar2 : MonoBehaviour
{
    [SerializeField] private Image barImageHealth;
    [SerializeField] private Image barImageDash;

    public void UpdateHealthBar(float maxHealth, float health)
    {
        barImageHealth.fillAmount = health / maxHealth;
    }

    public void UpdateDashBar(float timeDash)
    {
        barImageDash.fillAmount = timeDash;
    }
}
