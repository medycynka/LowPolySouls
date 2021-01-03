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
        public float currentHealth;
        public float currentStamina;
        public float baseArmor;
        public float Strength;
        public float Agility;
        public float Defence;
        public float bonusHealth;
        public float bonusStamina;
        public int playerLevel;
        public float soulsAmount;
        public float[] spawnPointPosition;
        public float[] spawnPointRotation;

        [System.Serializable]
        public struct TestItem
        {
            public int value_;
            public string name_;
        }

        public TestItem tI;

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

        public DataManager(PlayerManager playerManager, PlayerStats playerStats, PlayerInventory playerInventory, CurrentEquipments currentEquipments)
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

            baseArmor = playerStats.baseArmor;
            Strength = playerStats.Strength;
            Agility = playerStats.Agility;
            Defence = playerStats.Defence;
            bonusHealth = playerStats.bonusHealth;
            bonusStamina = playerStats.bonusStamina;
            playerLevel = playerStats.playerLevel;
            soulsAmount = playerStats.soulsAmount;

            spawnPointPosition = new float[3];
            spawnPointPosition[0] = playerManager.currentSpawnPoint.transform.position.x;
            spawnPointPosition[1] = playerManager.currentSpawnPoint.transform.position.y;
            spawnPointPosition[2] = playerManager.currentSpawnPoint.transform.position.z;

            spawnPointRotation = new float[3];
            spawnPointRotation[0] = playerManager.currentSpawnPoint.transform.rotation.eulerAngles.x;
            spawnPointRotation[1] = playerManager.currentSpawnPoint.transform.rotation.eulerAngles.y;
            spawnPointRotation[2] = playerManager.currentSpawnPoint.transform.rotation.eulerAngles.z;
        }
    }
}