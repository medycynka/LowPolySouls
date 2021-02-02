using UnityEngine;
using TMPro;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.GameUI.Slots;


namespace SzymonPeszek.GameUI.WindowsManagers
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
        public TextMeshProUGUI strengthValue;
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
            strengthValue.text = playerStats.strength.ToString();
            agilityValue.text = playerStats.agility.ToString();
            defenceValue.text = playerStats.defence.ToString();
            allDefenceValue.text = playerStats.currentArmorValue.ToString();
            healthValue.text = playerStats.bonusHealth.ToString();
            maxHealthValue.text = playerStats.maxHealth.ToString();
            staminaValue.text = playerStats.bonusStamina.ToString();
            maxStaminaValue.text = playerStats.maxStamina.ToString();
        }
    }

}