using UnityEngine;
using UnityEngine.UI;


namespace SzymonPeszek.GameUI.StatBars
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
