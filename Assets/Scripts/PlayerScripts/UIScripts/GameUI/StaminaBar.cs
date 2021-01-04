using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class StaminaBar : MonoBehaviour
    {
        public Slider staminaBarSlider;

        public void SetMaxStamina(float maxStamina)
        {
            staminaBarSlider.maxValue = maxStamina;
            staminaBarSlider.value = maxStamina;
        }

        public void SetCurrentStamina(float currentStamina)
        {
            staminaBarSlider.value = currentStamina;
        }
    }

}
