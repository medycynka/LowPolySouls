using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{
    public class HealthBar : MonoBehaviour
    {
        public Slider healthBarSlider;
        public Slider backgroundSlider;

        public void SetMaxHealth(float maxHealth)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = maxHealth;
            backgroundSlider.maxValue = maxHealth;
            backgroundSlider.value = maxHealth;
        }

        public void SetCurrentHealth(float currentHealth)
        {
            healthBarSlider.value = currentHealth;
        }
    }

}