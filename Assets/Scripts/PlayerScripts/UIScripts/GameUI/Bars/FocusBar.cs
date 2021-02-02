using UnityEngine;
using UnityEngine.UI;


namespace SzymonPeszek.GameUI.StatBars
{
    public class FocusBar : MonoBehaviour
    {
        public Slider focusBarSlider;

        public void SetMaxFocus(float maxFocus)
        {
            focusBarSlider.maxValue = maxFocus;
            focusBarSlider.value = maxFocus;
        }

        public void SetCurrentFocus(float currentFocus)
        {
            focusBarSlider.value = currentFocus;
        }
    }
}