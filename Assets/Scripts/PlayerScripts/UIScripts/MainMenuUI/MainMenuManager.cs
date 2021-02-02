using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.MainMenuUI {
    public class MainMenuManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Resolution[] _resolutionsOpts;

        [Header("Settings Manager Manager", order = 0)]
        [Header("Character Creator Components", order = 1)]
        public GameObject characterCreatorScreen;

        public float fadeOutTime = 2.5f;

        [Header("Settings Options", order = 1)]
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullScreenToggle;
        public TMP_Dropdown qualityDropdown;
        public Slider mouseSlider;
        public Slider volumeSlider;

        private float _startMusicVolume;
        private float _currentTime = 0.0f;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _resolutionsOpts = Screen.resolutions;
            SettingsHolder.qualityID = QualitySettings.GetQualityLevel();

            List<string> resList = new List<string>();
            for (int i = 0; i < _resolutionsOpts.Length; i++)
            {
                resList.Add(_resolutionsOpts[i].width + "x" + _resolutionsOpts[i].height);

                if (_resolutionsOpts[i].width == Screen.width && _resolutionsOpts[i].height == Screen.height)
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
            fullScreenToggle.isOn = SettingsHolder.isFullscreen;
            qualityDropdown.value = SettingsHolder.qualityID;
            mouseSlider.value = SettingsHolder.mouseSensibility;
            volumeSlider.value = SettingsHolder.soundVolume;
            _audioSource.volume = SettingsHolder.soundVolume;
            _startMusicVolume = SettingsHolder.soundVolume;

            resolutionDropdown.RefreshShownValue();

            SettingsHolder.dataManager = dataManager;
        }

        public void SaveSettings()
        {
            SettingsHolder.resolutionID = resolutionDropdown.value;
            SettingsHolder.isFullscreen = fullScreenToggle.isOn;
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
            Screen.SetResolution(_resolutionsOpts[resolutionId].width, _resolutionsOpts[resolutionId].height, Screen.fullScreen);
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
            _audioSource.volume = volume;
            _startMusicVolume = volume;
            SettingsHolder.soundVolume = volume;
        }

        private IEnumerator SwitchToNextScene()
        {
            while (_currentTime <= fadeOutTime)
            {
                _audioSource.volume = Mathf.Lerp(_startMusicVolume, 0.0f, _currentTime / fadeOutTime);
                _currentTime += Time.deltaTime;

                yield return null;
            }
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}