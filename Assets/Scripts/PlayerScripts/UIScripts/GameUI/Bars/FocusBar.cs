using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
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