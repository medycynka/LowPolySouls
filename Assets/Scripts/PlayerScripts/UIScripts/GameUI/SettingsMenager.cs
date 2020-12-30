using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SP
{
    public class SettingsMenager : MonoBehaviour
    {
        CameraHandler cameraHandler;
        List<AudioSource> audioSources = new List<AudioSource>();
        Resolution[] resolutionsOpts;

        [Header("Settings Menager Manager", order = 0)]
        [Header("Settings Options", order = 1)]
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullScreenToogle;
        public TMP_Dropdown qualityDropdown;
        public Slider mouseSlider;
        public Slider volumeSlider;

        private void Start()
        {
            cameraHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>();
            audioSources.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>());

            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (enemy.GetComponent<AudioSource>() != null)
                {
                    audioSources.Add(enemy.GetComponent<AudioSource>());
                }
            }

            resolutionsOpts = Screen.resolutions;
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

            LoadSettings();
        }

        private void LoadSettings()
        {
            resolutionDropdown.value = SettingsHolder.resolutionID;
            resolutionDropdown.RefreshShownValue();

            fullScreenToogle.isOn = SettingsHolder.isFullscreen;
            qualityDropdown.value = SettingsHolder.qualityID;
            qualityDropdown.RefreshShownValue();
            mouseSlider.value = SettingsHolder.mouseSensibility;
            volumeSlider.value = SettingsHolder.soundVolume;

            SetAllSounds(SettingsHolder.soundVolume);

            cameraHandler.lookSpeed = SettingsHolder.mouseSensibility / 1000f;
        }

        public void SaveSettings()
        {
            SettingsHolder.resolutionID = resolutionDropdown.value;
            SettingsHolder.isFullscreen = fullScreenToogle.isOn;
            SettingsHolder.qualityID = qualityDropdown.value;
            SettingsHolder.mouseSensibility = mouseSlider.value;
            SettingsHolder.soundVolume = volumeSlider.value;
        }

        public void SetResolution(int resolutionId)
        {
            Screen.SetResolution(resolutionsOpts[resolutionId].width, resolutionsOpts[resolutionId].height, Screen.fullScreen);
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
            cameraHandler.lookSpeed = (sensibility / 1000f);
        }

        public void SetVolume(float volume)
        {
            SetAllSounds(volume);
        }

        private void SetAllSounds(float volume)
        {
            foreach (var audioSource in audioSources)
            {
                audioSource.volume = volume;
            }
        }
    }
}