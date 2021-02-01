using SzymonPeszek.BaseClasses;
using SzymonPeszek.Items.Weapons;


namespace SzymonPeszek.GameUI.Slots
{

    public class HandEquipmentSlotUI : InventorySlotBase
    {
        private WeaponItem _weapon;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool leftHandSlot01;
        public bool leftHandSlot02;

        public void AddItem(WeaponItem newWeapon)
        {
            _weapon = newWeapon;
            icon.sprite = _weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            _weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            if (rightHandSlot01)
            {
                uiManager.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                uiManager.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                uiManager.leftHandSlot01Selected = true;
            }
            else
            {
                uiManager.leftHandSlot02Selected = true;
            }
        }
    }

}