using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BattleDrakeStudios.ModularCharacters;

namespace SP
{
    public class CharacterCreatorMenager : MonoBehaviour
    {
        [Header("Character Creator Manager", order = 0)]
        [Header("Character Creator Components", order = 1)]
        public ModularCharacterManager modularCharacterManager;
        public Slider headSlider;
        public Slider hairSlider;
        public Slider eyebrowSlider;
        public Slider earSlider;
        public Slider facialHairSlider;

        public void PlayGame()
        {
            SettingsHolder.isCharacterCreated = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void SetMaleGender()
        {
            if (!SettingsHolder.isMale)
            {
                SettingsHolder.isMale = true;
                modularCharacterManager.SwapGender(Gender.Male);
                eyebrowSlider.maxValue = 9;
                eyebrowSlider.value = -1;
            }
        }

        public void SetFemaleGender()
        {
            if (SettingsHolder.isMale)
            {
                SettingsHolder.isMale = false;
                modularCharacterManager.SwapGender(Gender.Female);
                eyebrowSlider.maxValue = 6;
                eyebrowSlider.value = -1;
            }
        }

        public void SetPlayerName(string name)
        {
            SettingsHolder.playerName = name;
        }

        public void SetHead(float partID)
        {
            SettingsHolder.headID = (int)partID;
            modularCharacterManager.ActivatePart(ModularBodyPart.Head, (int)partID);
        }

        public void SetHair(float partID)
        {
            SettingsHolder.headID = (int)partID;
            modularCharacterManager.ActivatePart(ModularBodyPart.Head, (int)partID);
        }

        public void SetEyebrow(float partID)
        {
            SettingsHolder.headID = (int)partID;
            modularCharacterManager.ActivatePart(ModularBodyPart.Head, (int)partID);
        }

        public void SetEar(float partID)
        {
            SettingsHolder.headID = (int)partID;
            modularCharacterManager.ActivatePart(ModularBodyPart.Head, (int)partID);
        }

        public void SetFacialHair(float partID)
        {
            SettingsHolder.headID = (int)partID;
            modularCharacterManager.ActivatePart(ModularBodyPart.Head, (int)partID);
        }
    }
}