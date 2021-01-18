using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SP
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponSlotManager weaponSlotManager;

        [Header("Inventory", order = 0)]
        [Header("Weapon On Load", order = 1)]
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public WeaponItem unarmedWeapon;

        [Header("Current Spell", order = 1)] 
        public SpellItem currentSpell;

        [Header("Weapons In Slots", order = 1)]
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

        [Header("Current Weapon Index", order = 1)]
        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;

        [Header("Items Lists", order = 1)]
        public List<WeaponItem> weaponsInventory;
        public List<WeaponItem> shieldsInventory;
        public List<EquipmentItem> helmetsInventory;
        public List<EquipmentItem> chestsInventory;
        public List<EquipmentItem> shouldersInventory;
        public List<EquipmentItem> handsInventory;
        public List<EquipmentItem> legsInventory;
        public List<EquipmentItem> feetInventory;
        public List<EquipmentItem> ringsInventory;
        public List<ConsumableItem> consumablesInventory;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            #region Inventory Initialization
            var dataManager = SaveManager.LoadGame();
            var worldManager = FindObjectOfType<WorldManager>();
            
            if (dataManager != null)
            {
                if (!dataManager.isFirstStart)
                {
                    #region Quick Slots Initialization
                    weaponsInRightHandSlots = new WeaponItem[2];
                    weaponsInRightHandSlots[0] = dataManager.rightHandSlots[0] == 0 ? worldManager.weaponsHolder[dataManager.rightHandSlots[1]] : worldManager.shieldsHolder[dataManager.rightHandSlots[1]];
                    weaponsInRightHandSlots[1] = dataManager.rightHandSlots[2] == 0 ? worldManager.weaponsHolder[dataManager.rightHandSlots[3]] : worldManager.shieldsHolder[dataManager.rightHandSlots[3]];
                    
                    weaponsInLeftHandSlots = new WeaponItem[2];
                    weaponsInLeftHandSlots[0] = dataManager.leftHandSlots[0] == 0 ? worldManager.weaponsHolder[dataManager.leftHandSlots[1]] : worldManager.shieldsHolder[dataManager.leftHandSlots[1]];
                    weaponsInLeftHandSlots[1] = dataManager.leftHandSlots[2] == 0 ? worldManager.weaponsHolder[dataManager.leftHandSlots[3]] : worldManager.shieldsHolder[dataManager.leftHandSlots[3]];
                    #endregion
                    
                    #region Inventory Initialization
                    #region Weapons
                    weaponsInventory = new List<WeaponItem>();
                    for (var i = 0; i < dataManager.weaponIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.weaponIds[i]; j++)
                        {
                            weaponsInventory.Add(worldManager.weaponsHolder[dataManager.weaponIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Shields
                    shieldsInventory = new List<WeaponItem>();
                    for (var i = 0; i < dataManager.shieldIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.shieldIds[i]; j++)
                        {
                            shieldsInventory.Add(worldManager.shieldsHolder[dataManager.shieldIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Helmets
                    helmetsInventory = new List<EquipmentItem>();
                    for (var i = 0; i < dataManager.helmetIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.helmetIds[i]; j++)
                        {
                            helmetsInventory.Add(worldManager.helmetsHolder[dataManager.helmetIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Chest Armor
                    chestsInventory = new List<EquipmentItem>();
                    for (var i = 0; i < dataManager.chestIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.chestIds[i]; j++)
                        {
                            chestsInventory.Add(worldManager.chestsHolder[dataManager.chestIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Shoulders
                    shouldersInventory = new List<EquipmentItem>();
                    for (var i = 0; i < dataManager.shoulderIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.shoulderIds[i]; j++)
                        {
                            shouldersInventory.Add(worldManager.shouldersHolder[dataManager.shoulderIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Hands
                    handsInventory = new List<EquipmentItem>();
                    for (var i = 0; i < dataManager.handIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.handIds[i]; j++)
                        {
                            handsInventory.Add(worldManager.handsHolder[dataManager.handIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Legs
                    legsInventory = new List<EquipmentItem>();
                    for (var i = 0; i < dataManager.legIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.legIds[i]; j++)
                        {
                            legsInventory.Add(worldManager.legsHolder[dataManager.legIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Feet
                    feetInventory = new List<EquipmentItem>();
                    for (var i = 0; i < dataManager.footIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.footIds[i]; j++)
                        {
                            feetInventory.Add(worldManager.feetHolder[dataManager.footIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Ring
                    ringsInventory = new List<EquipmentItem>();
                    for (var i = 0; i < dataManager.ringIds.Length; i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.ringIds[i]; j++)
                        {
                            ringsInventory.Add(worldManager.ringsHolder[dataManager.ringIds[i-1]]);
                        }
                    }
                    #endregion
                    
                    #region Consumable
                    consumablesInventory = new List<ConsumableItem>();
                    for (var i = 0; i < dataManager.consumableIds.GetLength(0); i++)
                    {
                        i++;
                        for (var j = 0; j < dataManager.consumableIds[i]; j++)
                        {
                            consumablesInventory.Add(worldManager.consumableHolder[dataManager.consumableIds[i-1]]);
                        }
                    }
                    #endregion
                    #endregion
                }
            }
            #endregion
            
            rightWeapon = weaponsInRightHandSlots[0];
            leftWeapon = weaponsInLeftHandSlots[0];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex++;

            if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            {
                currentRightWeaponIndex++;
            }

            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
            {
                currentRightWeaponIndex++;
            }

            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            }
        }

        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex++;

            if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
            {
                currentLeftWeaponIndex++;
            }
            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
            {
                currentLeftWeaponIndex++;
            }

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            }
        }
    }

}
