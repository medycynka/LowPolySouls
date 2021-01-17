using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public bool isFirstStart;
        public string playerName;
        public bool isMale;

        // Player
        public int[] partsID;
        public float[] partsArmor;
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
        
        // Player Inventory
        public int[,] leftHandSlots;
        public int[,] rightHandSlots;
        public int[,] weaponIds;
        public int[,] shieldIds;
        public int[,] helmetIds;
        public int[,] chestIds;
        public int[,] shoulderIds;
        public int[,] handIds;
        public int[,] legIds;
        public int[,] footIds;
        public int[,] ringIds;
        public int[,] consumableIds;

        // Area Menagers
        public bool[] areaBossesAlive;
        public bool[] bonfireActivators;

        // Constructor
        public DataManager()
        {
            resolutionID = SettingsHolder.resolutionID;
            isFullscreen = SettingsHolder.isFullscreen;
            qualityID = SettingsHolder.qualityID;
            mouseSensibility = SettingsHolder.mouseSensibility;
            soundVolume = SettingsHolder.soundVolume;
            isCharacterCreated = SettingsHolder.isCharacterCreated;
            isFirstStart = SettingsHolder.firstStart;
            playerName = SettingsHolder.playerName;
            isMale = SettingsHolder.isMale;

            partsID = new int[SettingsHolder.partsID.Length];
            partsArmor = new float[SettingsHolder.partsID.Length];

            for(int i = 0; i < SettingsHolder.partsID.Length; i++)
            {
                partsID[i] = SettingsHolder.partsID[i];
                partsArmor[i] = SettingsHolder.partsArmor[i];
            }
        }

        public DataManager(PlayerManager playerManager, PlayerStats playerStats, PlayerInventory playerInventory)
        {
            #region Setting
            resolutionID = SettingsHolder.resolutionID;
            isFullscreen = SettingsHolder.isFullscreen;
            qualityID = SettingsHolder.qualityID;
            mouseSensibility = SettingsHolder.mouseSensibility;
            soundVolume = SettingsHolder.soundVolume;
            isCharacterCreated = SettingsHolder.isCharacterCreated;
            isFirstStart = SettingsHolder.firstStart;
            playerName = SettingsHolder.playerName;
            isMale = SettingsHolder.isMale;
            #endregion

            partsID = new int[SettingsHolder.partsID.Length];
            partsArmor = new float[SettingsHolder.partsID.Length];

            for (var i = 0; i < SettingsHolder.partsID.Length; i++)
            {
                partsID[i] = SettingsHolder.partsID[i];
                partsArmor[i] = SettingsHolder.partsArmor[i];
            }

            #region Stats
            baseArmor = playerStats.baseArmor;
            Strength = playerStats.Strength;
            Agility = playerStats.Agility;
            Defence = playerStats.Defence;
            bonusHealth = playerStats.bonusHealth;
            bonusStamina = playerStats.bonusStamina;
            playerLevel = playerStats.playerLevel;
            soulsAmount = playerStats.soulsAmount;

            spawnPointPosition = new float[3];
            var position = playerManager.currentSpawnPoint.transform.position;
            spawnPointPosition[0] = position.x;
            spawnPointPosition[1] = position.y;
            spawnPointPosition[2] = position.z;

            spawnPointRotation = new float[3];
            var rotation = playerManager.currentSpawnPoint.transform.rotation;
            spawnPointRotation[0] = rotation.eulerAngles.x;
            spawnPointRotation[1] = rotation.eulerAngles.y;
            spawnPointRotation[2] = rotation.eulerAngles.z;
            #endregion

            #region Area
            areaBossesAlive = new bool[1];
            for(int i = 0; i < areaBossesAlive.Length; i++)
            {
                areaBossesAlive[i] = SettingsHolder.bossAreaAlive[i];
            }

            bonfireActivators = new bool[3];
            for(int i = 0; i < bonfireActivators.Length; i++)
            {
                bonfireActivators[i] = SettingsHolder.bonfiresAcrivation[i];
            }
            #endregion
            
            #region Inventory
            #region Quick Slots
            leftHandSlots = new int[2, 2];
            leftHandSlots[0, 0] = playerInventory.weaponsInLeftHandSlots[0].meleeType == MeleeType.Shield ? 1 : 0;
            leftHandSlots[0, 1] = playerInventory.weaponsInLeftHandSlots[0].itemId;
            leftHandSlots[1, 0] = playerInventory.weaponsInLeftHandSlots[1].meleeType == MeleeType.Shield ? 1 : 0;
            leftHandSlots[1, 1] = playerInventory.weaponsInLeftHandSlots[1].itemId;
            
            rightHandSlots = new int[2, 2];
            rightHandSlots[0, 0] = playerInventory.weaponsInRightHandSlots[0].meleeType == MeleeType.Shield ? 1 : 0;
            rightHandSlots[0, 1] = playerInventory.weaponsInRightHandSlots[0].itemId;
            rightHandSlots[1, 0] = playerInventory.weaponsInRightHandSlots[1].meleeType == MeleeType.Shield ? 1 : 0;
            rightHandSlots[1, 1] = playerInventory.weaponsInRightHandSlots[1].itemId;
            #endregion

            #region Current Weapons in Inventory
            int count = 0;
            var currentWeapons = playerInventory.weaponsInventory.GroupBy(i => i.itemId).ToList();
            weaponIds = new int[currentWeapons.Count(), 2];
            foreach (var kvp in currentWeapons)
            {
                weaponIds[count, 0] = kvp.Key;
                weaponIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Shield in Inventory
            count = 0;
            currentWeapons = playerInventory.shieldsInventory.GroupBy(i => i.itemId).ToList();
            shieldIds = new int[currentWeapons.Count(), 2];
            foreach (var kvp in currentWeapons)
            {
                shieldIds[count, 0] = kvp.Key;
                shieldIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Helmets in Inventory
            count = 0;
            var currentEq = playerInventory.helmetsInventory.GroupBy(i => i.itemId).ToList();
            helmetIds = new int[currentEq.Count(), 2];
            foreach (var kvp in currentEq)
            {
                helmetIds[count, 0] = kvp.Key;
                helmetIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion

            #region Current Chest Armor in Inventory
            count = 0;
            currentEq = playerInventory.chestsInventory.GroupBy(i => i.itemId).ToList();
            chestIds = new int[currentEq.Count(), 2];
            foreach (var kvp in currentEq)
            {
                chestIds[count, 0] = kvp.Key;
                chestIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Shoulder Armor in Inventory
            count = 0;
            currentEq = playerInventory.shouldersInventory.GroupBy(i => i.itemId).ToList();
            shoulderIds = new int[currentEq.Count(), 2];
            foreach (var kvp in currentEq)
            {
                shoulderIds[count, 0] = kvp.Key;
                shoulderIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Hand Armor in Inventory
            count = 0;
            currentEq = playerInventory.handsInventory.GroupBy(i => i.itemId).ToList();
            handIds = new int[currentEq.Count(), 2];
            foreach (var kvp in currentEq)
            {
                handIds[count, 0] = kvp.Key;
                handIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Leg Armor in Inventory
            count = 0;
            currentEq = playerInventory.legsInventory.GroupBy(i => i.itemId).ToList();
            legIds = new int[currentEq.Count(), 2];
            foreach (var kvp in currentEq)
            {
                legIds[count, 0] = kvp.Key;
                legIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Foot Armor in Inventory
            count = 0;
            currentEq = playerInventory.feetInventory.GroupBy(i => i.itemId).ToList();
            footIds = new int[currentEq.Count(), 2];
            foreach (var kvp in currentEq)
            {
                footIds[count, 0] = kvp.Key;
                footIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Rings in Inventory
            count = 0;
            currentEq = playerInventory.ringsInventory.GroupBy(i => i.itemId).ToList();
            ringIds = new int[currentEq.Count(), 2];
            foreach (var kvp in currentEq)
            {
                ringIds[count, 0] = kvp.Key;
                ringIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Shoulder Armor in Inventory
            count = 0;
            var currentCons = playerInventory.consumablesInventory.GroupBy(i => i.itemId).ToList();
            consumableIds = new int[currentCons.Count(), 2];
            foreach (var kvp in currentCons)
            {
                consumableIds[count, 0] = kvp.Key;
                consumableIds[count, 1] = kvp.Count();
                count++;
            }
            #endregion

            #endregion
        }
    }

    // Works
    /*
    [System.Serializable]
    public class WeaponItemToSave
    {
        public int iconID;
        public string itemName;
        public ItemType itemType;
        
    }
    */
}