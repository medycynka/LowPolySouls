using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public GameObject weaponInventorySlotsParent;
        public GameObject shieldInventorySlotPrefab;
        public GameObject shieldInventorySlotsParent;
        public GameObject helmetInventorySlotsPrefab;
        public GameObject helmetInventorySlotsParent;
        public GameObject chestInventorySlotsPrefab;
        public GameObject chestInventorySlotsParent;
        public GameObject shoulderInventorySlotsPrefab;
        public GameObject shoulderInventorySlotsParent;
        public GameObject handInventorySlotsPrefab;
        public GameObject handInventorySlotsParent;
        public GameObject legInventorySlotsPrefab;
        public GameObject legInventorySlotsParent;
        public GameObject footInventorySlotsPrefab;
        public GameObject footInventorySlotsParent;
        public GameObject ringInventorySlotsPrefab;
        public GameObject ringInventorySlotsParent;
        public GameObject consumableInventorySlotsPrefab;
        public GameObject consumableInventorySlotsParent;

        [SerializeField] WeaponInventorySlot[] weaponInventorySlots;
        [SerializeField] WeaponInventorySlot[] shieldInventorySlots;
        [SerializeField] EquipmentInventorySlot[] helmetInventorySlots;
        [SerializeField] EquipmentInventorySlot[] chestInventorySlots;
        [SerializeField] EquipmentInventorySlot[] shoulderInventorySlots;
        [SerializeField] EquipmentInventorySlot[] handInventorySlots;
        [SerializeField] EquipmentInventorySlot[] legInventorySlots;
        [SerializeField] EquipmentInventorySlot[] footInventorySlots;
        [SerializeField] EquipmentInventorySlot[] ringInventorySlots;
        [SerializeField] ConsumableInventorySlot[] consumableInventorySlots;

        private void Start()
        {
            GetAllInventorySlots();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }

        public void UpdateUI()
        {
            GetAllInventorySlots();
            UpdateAllInventoryTabs();
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

        #region Manage Inventory Tabs

        public void ResetTabsSelection()
        {
            weaponInventorySlotsParent.SetActive(true);
            shieldInventorySlotsParent.SetActive(false);
            helmetInventorySlotsParent.SetActive(false);
            chestInventorySlotsParent.SetActive(false);
            shoulderInventorySlotsParent.SetActive(false);
            handInventorySlotsParent.SetActive(false);
            legInventorySlotsParent.SetActive(false);
            footInventorySlotsParent.SetActive(false);
            ringInventorySlotsParent.SetActive(false);
            consumableInventorySlotsParent.SetActive(false);
        }

        #region Get Current Items list
        public void GetWeaponInventorySlot()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        }

        public void GetShieldInventorySlot()
        {
            shieldInventorySlots = shieldInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        }

        public void GetHelmetInventorySlot()
        {
            helmetInventorySlots = helmetInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        public void GetChestInventorySlot()
        {
            chestInventorySlots = chestInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        public void GetShoulderInventorySlot()
        {
            shoulderInventorySlots = shoulderInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        public void GetHandInventorySlot()
        {
            handInventorySlots = handInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        public void GetLegInventorySlot()
        {
            legInventorySlots = legInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        public void GetFootInventorySlot()
        {
            footInventorySlots = footInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        public void GetRingInventorySlot()
        {
            ringInventorySlots = ringInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        public void GetConsumableInventorySlot()
        {
            consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumableInventorySlot>();
        }

        public void GetAllInventorySlots()
        {
            GetWeaponInventorySlot();
            GetShieldInventorySlot();
            GetHelmetInventorySlot();
            GetChestInventorySlot();
            GetShoulderInventorySlot();
            GetHandInventorySlot();
            GetLegInventorySlot();
            GetFootInventorySlot();
            GetRingInventorySlot();
            GetConsumableInventorySlot();
        }
        #endregion

        #region Update Inventory Tabs
        public void UpdateAllInventoryTabs()
        {
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
        
        public void UpdateWeaponInventory()
        {
            #region Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent.transform);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot(playerInventory.weaponsInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateShieldInventory()
        {
            #region Shield Inventory Slots
            for (int i = 0; i < shieldInventorySlots.Length; i++)
            {
                if (i < playerInventory.shieldsInventory.Count)
                {
                    if (shieldInventorySlots.Length < playerInventory.shieldsInventory.Count)
                    {
                        Instantiate(shieldInventorySlotPrefab, shieldInventorySlotsParent.transform);
                        shieldInventorySlots = shieldInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    shieldInventorySlots[i].AddItem(playerInventory.shieldsInventory[i]);
                }
                else
                {
                    shieldInventorySlots[i].ClearInventorySlot(playerInventory.shieldsInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateHelmetInventory()
        {
            #region Helmet Inventory Slots
            for (int i = 0; i < helmetInventorySlots.Length; i++)
            {
                if (i < playerInventory.helmetsInventory.Count)
                {
                    if (helmetInventorySlots.Length < playerInventory.helmetsInventory.Count)
                    {
                        Instantiate(helmetInventorySlotsPrefab, helmetInventorySlotsParent.transform);
                        helmetInventorySlots = helmetInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    helmetInventorySlots[i].AddItem(playerInventory.helmetsInventory[i]);
                    helmetInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    helmetInventorySlots[i].ClearInventorySlot(playerInventory.helmetsInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateChestInventory()
        {
            #region Chest Inventory Slots
            for (int i = 0; i < chestInventorySlots.Length; i++)
            {
                if (i < playerInventory.chestsInventory.Count)
                {
                    if (chestInventorySlots.Length < playerInventory.chestsInventory.Count)
                    {
                        Instantiate(chestInventorySlotsPrefab, chestInventorySlotsParent.transform);
                        chestInventorySlots = chestInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    chestInventorySlots[i].AddItem(playerInventory.chestsInventory[i]);
                    chestInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    chestInventorySlots[i].ClearInventorySlot(playerInventory.chestsInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateShoulderInventory()
        {
            #region Shoulder Inventory Slots
            for (int i = 0; i < shoulderInventorySlots.Length; i++)
            {
                if (i < playerInventory.shouldersInventory.Count)
                {
                    if (shoulderInventorySlots.Length < playerInventory.shouldersInventory.Count)
                    {
                        Instantiate(shoulderInventorySlotsPrefab, shoulderInventorySlotsParent.transform);
                        shoulderInventorySlots = shoulderInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    shoulderInventorySlots[i].AddItem(playerInventory.shouldersInventory[i]);
                    shoulderInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    shoulderInventorySlots[i].ClearInventorySlot(playerInventory.shouldersInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateHandInventory()
        {
            #region Hand Inventory Slots
            for (int i = 0; i < handInventorySlots.Length; i++)
            {
                if (i < playerInventory.handsInventory.Count)
                {
                    if (handInventorySlots.Length < playerInventory.handsInventory.Count)
                    {
                        Instantiate(handInventorySlotsPrefab, handInventorySlotsParent.transform);
                        handInventorySlots = handInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    handInventorySlots[i].AddItem(playerInventory.handsInventory[i]);
                    handInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    handInventorySlots[i].ClearInventorySlot(playerInventory.handsInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateLegInventory()
        {
            #region Leg Inventory Slots
            for (int i = 0; i < legInventorySlots.Length; i++)
            {
                if (i < playerInventory.legsInventory.Count)
                {
                    if (legInventorySlots.Length < playerInventory.legsInventory.Count)
                    {
                        Instantiate(legInventorySlotsPrefab, legInventorySlotsParent.transform);
                        legInventorySlots = legInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    legInventorySlots[i].AddItem(playerInventory.legsInventory[i]);
                    legInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    legInventorySlots[i].ClearInventorySlot(playerInventory.legsInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateFootInventory()
        {
            #region Foot Inventory Slots
            for (int i = 0; i < footInventorySlots.Length; i++)
            {
                if (i < playerInventory.feetInventory.Count)
                {
                    if (footInventorySlots.Length < playerInventory.feetInventory.Count)
                    {
                        Instantiate(footInventorySlotsPrefab, footInventorySlotsParent.transform);
                        footInventorySlots = footInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    footInventorySlots[i].AddItem(playerInventory.feetInventory[i]);
                    footInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    footInventorySlots[i].ClearInventorySlot(playerInventory.feetInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateRingInventory()
        {
            #region Ring Inventory Slots
            for (int i = 0; i < ringInventorySlots.Length; i++)
            {
                if (i < playerInventory.ringsInventory.Count)
                {
                    if (ringInventorySlots.Length < playerInventory.ringsInventory.Count)
                    {
                        Instantiate(ringInventorySlotsPrefab, ringInventorySlotsParent.transform);
                        ringInventorySlots = ringInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    ringInventorySlots[i].AddItem(playerInventory.ringsInventory[i]);
                    ringInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    ringInventorySlots[i].ClearInventorySlot(playerInventory.ringsInventory.Count == 0);
                }
            }
            #endregion
        }

        public void UpdateConsumableInventory()
        {
            #region Consumable Inventory Slots
            for (int i = 0; i < consumableInventorySlots.Length; i++)
            {
                if (i < playerInventory.consumablesInventory.Count)
                {
                    if (consumableInventorySlots.Length < playerInventory.consumablesInventory.Count)
                    {
                        Instantiate(consumableInventorySlotsPrefab, consumableInventorySlotsParent.transform);
                        consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumableInventorySlot>();
                    }
                    consumableInventorySlots[i].AddItem(playerInventory.consumablesInventory[i]);
                }
                else
                {
                    consumableInventorySlots[i].ClearInventorySlot(playerInventory.consumablesInventory.Count == 0);
                }
            }
            #endregion
        }
        #endregion

        #endregion
    }

}