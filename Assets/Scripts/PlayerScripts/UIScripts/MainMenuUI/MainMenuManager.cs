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

        public float fadeOutTime = 2.5f;

        [Header("Settings Options", order = 1)]
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullScreenToogle;
        public TMP_Dropdown qualityDropdown;
        public Slider mouseSlider;
        public Slider volumeSlider;

        float startMusicVolume;
        float currentTime = 0.0f;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
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
                SettingsHolder.partsID[2] = dataManager.partsID[2];
                SettingsHolder.partsID[5] = dataManager.partsID[5];
                SettingsHolder.partsID[6] = dataManager.partsID[6];
                SettingsHolder.partsID[7] = dataManager.partsID[7];
                SettingsHolder.partsID[8] = dataManager.partsID[8];
            }

            resolutionDropdown.value = SettingsHolder.resolutionID;
            fullScreenToogle.isOn = SettingsHolder.isFullscreen;
            qualityDropdown.value = SettingsHolder.qualityID;
            mouseSlider.value = SettingsHolder.mouseSensibility;
            volumeSlider.value = SettingsHolder.soundVolume;
            audioSource.volume = SettingsHolder.soundVolume;
            startMusicVolume = SettingsHolder.soundVolume;

            resolutionDropdown.RefreshShownValue();

            SettingsHolder.dataManager = dataManager;
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
                FadeOutMusic();
            }
            else
            {
                SettingsHolder.firstStart = true;
                characterCreatorScreen.SetActive(true);
            }
        }

        public void FadeOutMusic()
        {
            StartCoroutine(SwitchToNextScene());
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
            startMusicVolume = volume;
            SettingsHolder.soundVolume = volume;
        }

        private IEnumerator SwitchToNextScene()
        {
            while (currentTime <= fadeOutTime)
            {
                audioSource.volume = Mathf.Lerp(startMusicVolume, 0.0f, currentTime / fadeOutTime);
                currentTime += Time.deltaTime;

                yield return null;
            }
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}