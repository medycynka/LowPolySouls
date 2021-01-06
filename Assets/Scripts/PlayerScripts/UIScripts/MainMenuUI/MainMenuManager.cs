using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SP {
    public class MainMenuManager : MonoBehaviour
    {
        AudioSource audioSource;
        Resolution[] resolutionsOpts;

        [Header("Settings Menager Manager", order = 0)]
        [Header("Character Creator Components", order = 1)]
        public GameObject characterCreatorScreen;

        [Header("Settings Options", order = 1)]
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullScreenToogle;
        public TMP_Dropdown qualityDropdown;
        public Slider mouseSlider;
        public Slider volumeSlider;

        private void Start()
        {
            audioSource = GetComponentInParent<AudioSource>();
            resolutionsOpts = Screen.resolutions;
            SettingsHolder.qualityID = QualitySettings.GetQualityLevel();

            List<string> resList = new List<string>();
            for (int i = 0; i < resolutionsOpts.Length; i++)
            {
                resList.Add(resolutionsOpts[i].width + "x" + resolutionsOpts[i].height);

                if (resolutionsOpts[i].width == Screen.width && resolutionsOpts[i].height == Screen.height)
                {
                    SettingsHolder.resolutionID = i;
                }
            }

            resolutionDropdown.AddOptions(resList);

            DataManager dataManager = SaveManager.LoadGame();

            if (dataManager != null)
            {
                SettingsHolder.resolutionID = dataManager.resolutionID;
                SettingsHolder.isFullscreen = dataManager.isFullscreen;
                SettingsHolder.qualityID = dataManager.qualityID;
                SettingsHolder.mouseSensibility = dataManager.mouseSensibility;
                SettingsHolder.soundVolume = dataManager.soundVolume;
                SettingsHolder.isCharacterCreated = dataManager.isCharacterCreated;
                SettingsHolder.playerName = dataManager.playerName;
                SettingsHolder.isMale = dataManager.isMale;
                SettingsHolder.headID = dataManager.headID;
                SettingsHolder.hairID = dataManager.hairID;
                SettingsHolder.eyebrowID = dataManager.eyebrowID;
                SettingsHolder.earID = dataManager.earID;
                SettingsHolder.facialHairID = dataManager.facialHairID;
            }

            resolutionDropdown.value = SettingsHolder.resolutionID;
            fullScreenToogle.isOn = SettingsHolder.isFullscreen;
            qualityDropdown.value = SettingsHolder.qualityID;
            mouseSlider.value = SettingsHolder.mouseSensibility;
            volumeSlider.value = SettingsHolder.soundVolume;

            resolutionDropdown.RefreshShownValue();
        }

        public void SaveSettings()
        {
            SettingsHolder.resolutionID = resolutionDropdown.value;
            SettingsHolder.isFullscreen = fullScreenToogle.isOn;
            SettingsHolder.qualityID = qualityDropdown.value;
            SettingsHolder.mouseSensibility = mouseSlider.value;
            SettingsHolder.soundVolume = volumeSlider.value;
        }

        public void PlayGame()
        {
            if (SettingsHolder.isCharacterCreated)
            {
                SettingsHolder.firstStart = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SettingsHolder.firstStart = true;
                characterCreatorScreen.SetActive(true);
            }
        }

        public void QuitGame()
        {
            Debug.Log("Quitting the game...");
            Application.Quit();
        }

        public void SetResolution(int resolutionId)
        {
            Screen.SetResolution(resolutionsOpts[resolutionId].width, resolutionsOpts[resolutionId].height, Screen.fullScreen);
            SettingsHolder.resolutionID = resolutionId;
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            SettingsHolder.isFullscreen = isFullScreen;
        }

        public void SetQuality(int qualityId)
        {
            QualitySettings.SetQualityLevel(qualityId);
            SettingsHolder.qualityID = qualityId;
        }

        public void SetMouseSensibility(float sensibility)
        {
            SettingsHolder.mouseSensibility = sensibility;
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
            SettingsHolder.soundVolume = volume;
        }
    }
}