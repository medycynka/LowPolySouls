using System;
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
        MainMenuManager mainMenuManager;
        
        [Header("Character Creator Manager", order = 0)]
        [Header("Character Creator Components", order = 1)]
        public GameObject creatorScreen;
        public ModularCharacterManager modularCharacterManager;
        public Slider headSlider;
        public Slider hairSlider;
        public Slider eyebrowSlider;
        public Slider earSlider;
        public Slider facialHairSlider;

        private void Start()
        {
            mainMenuManager = GameObject.FindObjectOfType<MainMenuManager>();
        }

        public void PlayGame()
        {
            SettingsHolder.isCharacterCreated = true;
            SaveManager.SaveMainMenu();
            creatorScreen.SetActive(false);
            
            mainMenuManager.FadeOutMusic();
        }

        public void SetMaleGender()
        {
            if (!SettingsHolder.isMale)
            {
                SettingsHolder.isMale = true;
                modularCharacterManager.SwapGender(Gender.Male);

                headSlider.value = 0;
                hairSlider.value = -1;
                eyebrowSlider.maxValue = 9;
                eyebrowSlider.value = -1;
                earSlider.value = -1;
                facialHairSlider.value = -1;
            }
        }

        public void SetFemaleGender()
        {
            if (SettingsHolder.isMale)
            {
                SettingsHolder.isMale = false;
                modularCharacterManager.SwapGender(Gender.Female);

                headSlider.value = 0;
                hairSlider.value = -1;
                eyebrowSlider.maxValue = 6;
                eyebrowSlider.value = -1;
                earSlider.value = -1;
                facialHairSlider.value = -1;
            }
        }

        public void SetPlayerName(string name)
        {
            SettingsHolder.playerName = name;
        }

        public void SetHead(float partID)
        {
            SettingsHolder.partsID[2] = (int)partID;
            modularCharacterManager.ActivatePart(ModularBodyPart.Head, (int)partID);
        }

        public void SetHair(float partID)
        {
            SettingsHolder.partsID[5] = (int)partID;

            if (partID > -1)
            {
                modularCharacterManager.ActivatePart(ModularBodyPart.Hair, (int)partID);
            }
            else
            {
                modularCharacterManager.DeactivatePart(ModularBodyPart.Hair);
            }
        }

        public void SetEyebrow(float partID)
        {
            SettingsHolder.partsID[6] = (int)partID;

            if (partID > -1)
            {
                modularCharacterManager.ActivatePart(ModularBodyPart.Eyebrow, (int)partID);
            }
            else
            {
                modularCharacterManager.DeactivatePart(ModularBodyPart.Eyebrow);
            }
        }

        public void SetEar(float partID)
        {
            SettingsHolder.partsID[7] = (int)partID;

            if (partID > -1)
            {
                modularCharacterManager.ActivatePart(ModularBodyPart.Ear, (int)partID);
            }
            else
            {
                modularCharacterManager.DeactivatePart(ModularBodyPart.Ear);
            }
        }

        public void SetFacialHair(float partID)
        {
            SettingsHolder.partsID[8] = (int)partID;

            if (partID > -1)
            {
                modularCharacterManager.ActivatePart(ModularBodyPart.FacialHair, (int)partID);
            }
            else
            {
                modularCharacterManager.DeactivatePart(ModularBodyPart.FacialHair);
            }
        }
    }
}