using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    //public Gradient gradient;
    //public Image fill;

    public void InitializeHealthBar(float maxHealth, float health)
    {
        slider.minValue = 0;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
