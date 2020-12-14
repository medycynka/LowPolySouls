using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SP
{

    public class EquipmentWindowUI : MonoBehaviour
    {
        [Header("Weapon Quick Slots")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public HandEquipmentSlotUI[] handEquipmentSlotUI;

        [Header("Stats Values")]
        public TextMeshProUGUI strenghtValue;
        public TextMeshProUGUI agilityValue;
        public TextMeshProUGUI defenceValue;
        public TextMeshProUGUI allDefenceValue;
        public TextMeshProUGUI healthValue;
        public TextMeshProUGUI maxHealthValue;
        public TextMeshProUGUI staminaValue;
        public TextMeshProUGUI maxStaminaValue;

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                if (handEquipmentSlotUI[i].rightHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (handEquipmentSlotUI[i].rightHandSlot02)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }

        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }

        public void UpdateStatsWindow(PlayerStats playerStats)
        {
            strenghtValue.text = playerStats.Strength.ToString();
            agilityValue.text = playerStats.Agility.ToString();
            defenceValue.text = playerStats.Defence.ToString();
            allDefenceValue.text = playerStats.currentArmorValue.ToString();
            healthValue.text = playerStats.bonusHealth.ToString();
            maxHealthValue.text = playerStats.maxHealth.ToString();
            staminaValue.text = playerStats.bonusStamina.ToString();
            maxStaminaValue.text = playerStats.maxStamina.ToString();
        }
    }

}