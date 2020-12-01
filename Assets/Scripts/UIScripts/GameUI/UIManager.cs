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
        public GameObject uiWindow;
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
        public GameObject shieldInventorySlotPrefab;
        public Transform shieldInventorySlotsParent;
        public GameObject helmetInventorySlotsPrefab;
        public Transform helmetInventorySlotsParent;
        public GameObject chestInventorySlotsPrefab;
        public Transform chestInventorySlotsParent;
        public GameObject shoulderInventorySlotsPrefab;
        public Transform shoulderInventorySlotsParent;
        public GameObject handInventorySlotsPrefab;
        public Transform handInventorySlotsParent;
        public GameObject legInventorySlotsPrefab;
        public Transform legInventorySlotsParent;
        public GameObject footInventorySlotsPrefab;
        public Transform footInventorySlotsParent;
        public GameObject ringInventorySlotsPrefab;
        public Transform ringInventorySlotsParent;
        public GameObject consumableInventorySlotsPrefab;
        public Transform consumableInventorySlotsParent;

        WeaponInventorySlot[] weaponInventorySlots;
        WeaponInventorySlot[] shieldInventorySlots;
        EquipmentInventorySlot[] helmetInventorySlots;
        EquipmentInventorySlot[] chestInventorySlots;
        EquipmentInventorySlot[] shoulderInventorySlots;
        EquipmentInventorySlot[] handInventorySlots;
        EquipmentInventorySlot[] legInventorySlots;
        EquipmentInventorySlot[] footInventorySlots;
        EquipmentInventorySlot[] ringInventorySlots;
        EquipmentInventorySlot[] consumableInventorySlots;

        private void Awake()
        {
            
        }

        private void Start()
        {
            GetInventorySlots();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }

        public void UpdateUI()
        {
            GetInventorySlots();
            UpdateWeaponInventory();
            UpdateShieldInventory();
            UpdateHelmetInventory();
            UpdateChestInventory();
            UpdateShoulderInventory();
            UpdateHandInventory();
            UpdateLegInventory();
            UpdateFootInventory();
            UpdateRingInventory();
            UpdateConsumableInventory();
        }

        public void OpenSelectWindow()
        {
            uiWindow.SetActive(true);
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
            uiWindow.SetActive(false);
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

        #region Update Inventory Tabs
        private void GetInventorySlots()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            shieldInventorySlots = shieldInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            helmetInventorySlots = helmetInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
            chestInventorySlots = chestInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
            shoulderInventorySlots = shoulderInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
            handInventorySlots = handInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
            legInventorySlots = legInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
            footInventorySlots = footInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
            ringInventorySlots = ringInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
            consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        private void UpdateWeaponInventory()
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
        }

        private void UpdateShieldInventory()
        {
            #region Shield Inventory Slots
            for (int i = 0; i < shieldInventorySlots.Length; i++)
            {
                if (i < playerInventory.shieldsInventory.Count)
                {
                    if (shieldInventorySlots.Length < playerInventory.shieldsInventory.Count)
                    {
                        Instantiate(shieldInventorySlotPrefab, shieldInventorySlotsParent);
                        shieldInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    shieldInventorySlots[i].AddItem(playerInventory.shieldsInventory[i]);
                }
                else
                {
                    shieldInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        private void UpdateHelmetInventory()
        {
            #region Helmet Inventory Slots
            for (int i = 0; i < helmetInventorySlots.Length; i++)
            {
                if (i < playerInventory.helmetsInventory.Count)
                {
                    if (helmetInventorySlots.Length < playerInventory.helmetsInventory.Count)
                    {
                        Instantiate(helmetInventorySlotsPrefab, helmetInventorySlotsParent);
                        helmetInventorySlots = helmetInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    helmetInventorySlots[i].AddItem(playerInventory.helmetsInventory[i]);
                    helmetInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    helmetInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        private void UpdateChestInventory()
        {
            #region Chest Inventory Slots
            for (int i = 0; i < chestInventorySlots.Length; i++)
            {
                if (i < playerInventory.chestsInventory.Count)
                {
                    if (chestInventorySlots.Length < playerInventory.chestsInventory.Count)
                    {
                        Instantiate(chestInventorySlotsPrefab, chestInventorySlotsParent);
                        chestInventorySlots = chestInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    chestInventorySlots[i].AddItem(playerInventory.chestsInventory[i]);
                    chestInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    chestInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        private void UpdateShoulderInventory()
        {
            #region Shoulder Inventory Slots
            for (int i = 0; i < shoulderInventorySlots.Length; i++)
            {
                if (i < playerInventory.shouldersInventory.Count)
                {
                    if (shoulderInventorySlots.Length < playerInventory.shouldersInventory.Count)
                    {
                        Instantiate(shoulderInventorySlotsPrefab, shoulderInventorySlotsParent);
                        shoulderInventorySlots = shoulderInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    shoulderInventorySlots[i].AddItem(playerInventory.shouldersInventory[i]);
                    shoulderInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    shoulderInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        private void UpdateHandInventory()
        {
            #region Hand Inventory Slots
            for (int i = 0; i < handInventorySlots.Length; i++)
            {
                if (i < playerInventory.handsInventory.Count)
                {
                    if (handInventorySlots.Length < playerInventory.handsInventory.Count)
                    {
                        Instantiate(handInventorySlotsPrefab, handInventorySlotsParent);
                        handInventorySlots = handInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    handInventorySlots[i].AddItem(playerInventory.handsInventory[i]);
                    handInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    handInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        private void UpdateLegInventory()
        {
            #region Leg Inventory Slots
            for (int i = 0; i < legInventorySlots.Length; i++)
            {
                if (i < playerInventory.legsInventory.Count)
                {
                    if (legInventorySlots.Length < playerInventory.legsInventory.Count)
                    {
                        Instantiate(legInventorySlotsPrefab, legInventorySlotsParent);
                        legInventorySlots = legInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    legInventorySlots[i].AddItem(playerInventory.legsInventory[i]);
                    legInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    legInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        private void UpdateFootInventory()
        {
            #region Foot Inventory Slots
            for (int i = 0; i < footInventorySlots.Length; i++)
            {
                if (i < playerInventory.feetInventory.Count)
                {
                    if (footInventorySlots.Length < playerInventory.feetInventory.Count)
                    {
                        Instantiate(footInventorySlotsPrefab, footInventorySlotsParent);
                        footInventorySlots = footInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    footInventorySlots[i].AddItem(playerInventory.feetInventory[i]);
                    footInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    footInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        private void UpdateRingInventory()
        {
            #region Ring Inventory Slots
            for (int i = 0; i < ringInventorySlots.Length; i++)
            {
                if (i < playerInventory.ringsInventory.Count)
                {
                    if (ringInventorySlots.Length < playerInventory.ringsInventory.Count)
                    {
                        Instantiate(ringInventorySlotsPrefab, ringInventorySlotsParent);
                        ringInventorySlots = ringInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    ringInventorySlots[i].AddItem(playerInventory.ringsInventory[i]);
                    ringInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    ringInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        private void UpdateConsumableInventory()
        {
            #region Consumable Inventory Slots
            for (int i = 0; i < consumableInventorySlots.Length; i++)
            {
                if (i < playerInventory.consumablesInventory.Count)
                {
                    if (consumableInventorySlots.Length < playerInventory.consumablesInventory.Count)
                    {
                        Instantiate(consumableInventorySlotsPrefab, consumableInventorySlotsParent);
                        consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    consumableInventorySlots[i].AddItem(playerInventory.consumablesInventory[i]);
                    consumableInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    consumableInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }
        #endregion
    }

}