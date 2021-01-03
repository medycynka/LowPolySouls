using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    [System.Serializable]
    public class DataManager
    {
        // Settings
        public int resolutionID;
        public bool isFullscreen;
        public int qualityID;
        public float mouseSensibility;
        public float soundVolume;

        // Main Menu
        public bool isCharacterCreated;
        public string playerName;
        public bool isMale;
        public int headID;
        public int hairID;
        public int eyebrowID;
        public int earID;
        public int facialHairID;

        // Player

        // Constructor
        public DataManager()
        {
            resolutionID = SettingsHolder.resolutionID;
            isFullscreen = SettingsHolder.isFullscreen;
            qualityID = SettingsHolder.qualityID;
            mouseSensibility = SettingsHolder.mouseSensibility;
            soundVolume = SettingsHolder.soundVolume;
            isCharacterCreated = SettingsHolder.isCharacterCreated;
            playerName = SettingsHolder.playerName;
            isMale = SettingsHolder.isMale;
            headID = SettingsHolder.headID;
            hairID = SettingsHolder.hairID;
            eyebrowID = SettingsHolder.eyebrowID;
            earID = SettingsHolder.earID;
            facialHairID = SettingsHolder.facialHairID;
        }

        public DataManager(PlayerStats playerStats, PlayerInventory playerInventory, CurrentEquipments currentEquipments)
        {

        }
    }
}