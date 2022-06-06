using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Health bar manager
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    /// <summary>
    /// Initialize the slider's values
    /// </summary>
    /// <param name="maxHealth">Maximum health</param>
    /// <param name="health">Current health</param>
    public void InitializeHealthBar(float maxHealth, float health)
    {
        slider.minValue = 0;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    /// <summary>
    /// Set up the new health of the player
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
