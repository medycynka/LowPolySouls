using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleDrakeStudios.ModularCharacters;

public class ColorButtonPicker : MonoBehaviour
{
    public ModularCharacterManager characterManager;
    public string colorProperty;
    public Image buttonImage;

    private void Awake()
    {
        Color tempColor = characterManager.CharacterMaterial.GetColor(colorProperty);
        buttonImage.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1.0f);
    }
}
