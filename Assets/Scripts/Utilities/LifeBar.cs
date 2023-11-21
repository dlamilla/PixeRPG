using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    public void UpdateLifeBar(float maxLife, float LifeCurrent)
    {
        barImage.fillAmount = LifeCurrent / maxLife;
    }
}
