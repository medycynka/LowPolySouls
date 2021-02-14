using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattleDrakeStudios.ModularCharacters;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.MainMenuUI
{
    public class CharacterCreatorManager : MonoBehaviour
    {
        private MainMenuManager _mainMenuManager;
        
        [Header("Character Creator Manager", order = 0)]
        [Header("Character Creator Components", order = 1)]
        public GameObject creatorScreen;
        public ModularCharacterManager modularCharacterManager;
        public Slider headSlider;
        public Slider hairSlider;
        public Slider eyebrowSlider;
        public Slider earSlider;
        public Slider facialHairSlider;

        [Header("Character Stats Components", order = 1)]
        public int currentLevel = 1;
        public int pointsToSpend = 11;
        public float startStrength = 1f;
        public float startAgility;
        public float startDefence;
        public float startBonusHealth;
        public float startBonusStamina;
        public float startBonusFocus;
        public TextMeshProUGUI pointsToSpendText;
        public TextMeshProUGUI strengthText;
        public TextMeshProUGUI agilityText;
        public TextMeshProUGUI defenceText;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI staminaText;
        public TextMeshProUGUI focusText;

        private void Start()
        {
            _mainMenuManager = GetComponentInParent<MainMenuManager>();
            
            strengthText.text = startStrength.ToString();
            agilityText.text = startAgility.ToString();
            defenceText.text = startDefence.ToString();
            healthText.text = startBonusHealth.ToString();
            staminaText.text = startBonusStamina.ToString();
            focusText.text = startBonusFocus.ToString();
            pointsToSpendText.text = pointsToSpend.ToString();
        }

        public void PlayGame()
        {
            if (currentLevel == 12 && pointsToSpend == 0)
            {
                SaveStartingStats();
                SettingsHolder.isCharacterCreated = true;
                SaveManager.SaveMainMenu();
                creatorScreen.SetActive(false);

                _mainMenuManager.FadeOutMusic();
            }
        }

        private void SaveStartingStats()
        {
            SettingsHolder.currentLevel = currentLevel;
            SettingsHolder.currentStrength = startStrength;
            SettingsHolder.currentAgility = startAgility;
            SettingsHolder.currentDefence = startDefence;
            SettingsHolder.currentBonusHealth = startBonusHealth;
            SettingsHolder.currentBonusStamina = startBonusStamina;
            SettingsHolder.currentBonusFocus = startBonusFocus;
        }

        #region Character Appearance
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

        public void SetPlayerName(string newName)
        {
            SettingsHolder.playerName = newName;
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
        #endregion

        #region Character Stats
        public void IncreaseStrength()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startStrength += 1f;
                pointsToSpend--;

                strengthText.text = startStrength.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void DecreaseStrength()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startStrength > 0)
            {
                currentLevel--;
                startStrength -= 1f;
                pointsToSpend++;
                
                strengthText.text = startStrength.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void IncreaseAgility()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startAgility += 1f;
                pointsToSpend--;
                
                agilityText.text = startAgility.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void DecreaseAgility()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startAgility > 0)
            {
                currentLevel--;
                startAgility -= 1f;
                pointsToSpend++;
                
                agilityText.text = startAgility.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void IncreaseDefence()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startDefence += 1f;
                pointsToSpend--;
                
                defenceText.text = startDefence.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void DecreaseDefence()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startDefence > 0)
            {
                currentLevel--;
                startDefence -= 1f;
                pointsToSpend++;
                
                defenceText.text = startDefence.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void IncreaseHealth()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startBonusHealth += 1f;
                pointsToSpend--;
                
                healthText.text = startBonusHealth.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void DecreaseHealth()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startBonusHealth > 0)
            {
                currentLevel--;
                startBonusHealth -= 1f;
                pointsToSpend++;
                
                healthText.text = startBonusHealth.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void IncreaseStamina()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startBonusStamina += 1f;
                pointsToSpend--;
                
                staminaText.text = startBonusStamina.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void DecreaseStamina()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startBonusStamina > 0)
            {
                currentLevel--;
                startBonusStamina -= 1f;
                pointsToSpend++;
                
                staminaText.text = startBonusStamina.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void IncreaseFocus()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startBonusFocus += 1f;
                pointsToSpend--;
                
                focusText.text = startBonusFocus.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        public void DecreaseFocus()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startBonusFocus > 0)
            {
                currentLevel--;
                startBonusFocus -= 1f;
                pointsToSpend++;
                
                focusText.text = startBonusFocus.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        #endregion
    }
}