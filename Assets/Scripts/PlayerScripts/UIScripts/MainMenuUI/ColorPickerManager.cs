using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleDrakeStudios.ModularCharacters;
using TMPro;

namespace SP
{
    public class ColorPickerManager : MonoBehaviour
    {
        [Header("Color Picker Manager", order = 0)]
        [Header("Color Picker Components", order = 1)]
        public ModularCharacterManager modularCharacterManager;
        public string colorPropery = "";

        [Header("Color Properties", order = 1)]
        public Color partColor;
        public Image shownColor;
        public Image buttonColor;
        public float colorR = 1.0f;
        public float colorG = 1.0f;
        public float colorB = 1.0f;
        public float colorA = 1.0f;
        private Color baseColor;

        [Header("Sliders", order = 1)]
        public Slider rSlider;
        public Slider gSlider;
        public Slider bSlider;
        public Slider aSlider;

        private void Awake()
        {
            InitializeContent();
        }

        public void InitializeContent()
        {
            partColor = modularCharacterManager.CharacterMaterial.GetColor(colorPropery);
            shownColor.color = partColor;
            colorR = partColor.r;
            colorG = partColor.g;
            colorB = partColor.b;
            colorA = partColor.a;
            rSlider.value = colorR;
            gSlider.value = colorG;
            bSlider.value = colorB;
            aSlider.value = colorA;
            baseColor = partColor;
            buttonColor.color = partColor;
        }

        public void SetColorR(float r_)
        {
            colorR = r_;

            SetColor();
        }

        public void GetColorRValue(string s_)
        {
            if (s_ == "")
            {
                colorR = 0.0f;
            }
            else
            {
                colorR = float.Parse(s_) / 255.0f;
            }

            rSlider.value = colorR;

            SetColor();
        }

        public void SetColorG(float g_)
        {
            colorG = g_;

            SetColor();
        }

        public void GetColorGValue(string s_)
        {
            if (s_ == "")
            {
                colorG = 0.0f;
            }
            else
            {
                colorG = float.Parse(s_) / 255.0f;
            }

            gSlider.value = colorG;

            SetColor();
        }

        public void SetColorB(float b_)
        {
            colorB = b_;

            SetColor();
        }

        public void GetColorBValue(string s_)
        {
            if (s_ == "")
            {
                colorB = 0.0f;
            }
            else
            {
                colorB = float.Parse(s_) / 255.0f;
            }

            bSlider.value = colorB;

            SetColor();
        }

        public void SetColorA(float a_)
        {
            colorA = a_;

            SetColor();
        }

        public void GetColorAValue(string s_)
        {
            if (s_ == "")
            {
                colorA = 0.0f;
            }
            else
            {
                colorA = float.Parse(s_) / 255.0f;
            }

            aSlider.value = colorA;

            SetColor();
        }

        public void SaveColor()
        {
            buttonColor.color = new Color(partColor.r, partColor.g, partColor.b, 1.0f);
        }

        public void ResetColor()
        {
            partColor = baseColor;
            buttonColor.color = new Color(partColor.r, partColor.g, partColor.b, 1.0f);

            Material mat_ = modularCharacterManager.CharacterMaterial;
            mat_.SetColor(colorPropery, partColor);
            modularCharacterManager.SetAllPartsMaterial(mat_);
        }

        private void SetColor()
        {
            partColor = new Color(colorR, colorG, colorB, colorA);
            shownColor.color = partColor;

            Material mat_ = modularCharacterManager.CharacterMaterial;
            mat_.SetColor(colorPropery, partColor);
            modularCharacterManager.SetAllPartsMaterial(mat_);
        }
    }
}