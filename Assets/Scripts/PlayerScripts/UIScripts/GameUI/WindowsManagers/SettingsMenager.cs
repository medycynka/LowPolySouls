using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.PlayerScripts.CameraManager;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    public class SettingsMenager : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private CameraHandler _cameraHandler;
        private readonly List<AudioSource> _audioSources = new List<AudioSource>();
        private Resolution[] _resolutionsOpts;

        [Header("Settings Manager Manager", order = 0)]
        [Header("Settings Options", order = 1)]
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullScreenToggle;
        public TMP_Dropdown qualityDropdown;
        public Slider mouseSlider;
        public Slider volumeSlider;

        private void Start()
        {
            _playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            _cameraHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>();
            _audioSources.Add(_playerManager.GetComponent<AudioSource>());

            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (enemy.GetComponent<AudioSource>() != null)
                {
                    _audioSources.Add(enemy.GetComponent<AudioSource>());
                }
            }

            _resolutionsOpts = Screen.resolutions;
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

            LoadSettings();
        }

        private void LoadSettings()
        {
            resolutionDropdown.value = SettingsHolder.resolutionID;
            resolutionDropdown.RefreshShownValue();

            fullScreenToggle.isOn = SettingsHolder.isFullscreen;
            qualityDropdown.value = SettingsHolder.qualityID;
            qualityDropdown.RefreshShownValue();
            mouseSlider.value = SettingsHolder.mouseSensibility;
            volumeSlider.value = SettingsHolder.soundVolume;

            SetAllSounds(SettingsHolder.soundVolume);

            _cameraHandler.lookSpeed = SettingsHolder.mouseSensibility / 1000f;
        }

        public void SaveSettings()
        {
            SettingsHolder.resolutionID = resolutionDropdown.value;
            SettingsHolder.isFullscreen = fullScreenToggle.isOn;
            SettingsHolder.qualityID = qualityDropdown.value;
            SettingsHolder.mouseSensibility = mouseSlider.value;
            SettingsHolder.soundVolume = volumeSlider.value;
        }

        public void SetResolution(int resolutionId)
        {
            Screen.SetResolution(_resolutionsOpts[resolutionId].width, _resolutionsOpts[resolutionId].height, Screen.fullScreen);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        public void SetQuality(int qualityId)
        {
            QualitySettings.SetQualityLevel(qualityId);
        }

        public void SetMouseSensibility(float sensibility)
        {
            _cameraHandler.lookSpeed = (sensibility / 1000f);
        }

        public void SetVolume(float volume)
        {
            SetAllSounds(volume);
        }

        public void SaveAndExit()
        {
            SaveManager.SaveGame(_playerManager, _playerManager.GetComponent<PlayerStats>(), _playerManager.GetComponent<PlayerInventory>());
            Application.Quit();
        }

        private void SetAllSounds(float volume)
        {
            foreach (var audioSource in _audioSources)
            {
                audioSource.volume = volume;
            }
        }
    }
}