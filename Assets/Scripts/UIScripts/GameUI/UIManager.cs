using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        [Header("Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        public GameObject itemInventorySlotsPrefab;
        public Transform itemInventorySlotsParent;

        WeaponInventorySlot[] weaponInventorySlots;
        EquipmentInventorySlot[] equipmentInventorySlots;

        private void Awake()
        {
            
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentInventorySlots = itemInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Equipment Inventory Slots
            for (int i = 0; i < equipmentInventorySlots.Length; i++)
            {
                if (i < playerInventory.equipmentInventory.Count)
                {
                    if (equipmentInventorySlots.Length < playerInventory.equipmentInventory.Count)
                    {
                        Instantiate(itemInventorySlotsPrefab, itemInventorySlotsParent);
                        equipmentInventorySlots = itemInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    equipmentInventorySlots[i].AddItem(playerInventory.equipmentInventory[i]);
                    equipmentInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    equipmentInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }
    }

}