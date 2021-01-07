using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP {
    public class InventoryHolder : MonoBehaviour
    {
        public List<WeaponItem> weaponsInventory;
        public int[] weaponAmount;
        public List<WeaponItem> shieldsInventory;
        public List<EquipmentItem> helmetsInventory;
        public List<EquipmentItem> chestsInventory;
        public List<EquipmentItem> shouldersInventory;
        public List<EquipmentItem> handsInventory;
        public List<EquipmentItem> legsInventory;
        public List<EquipmentItem> feetInventory;
        public List<EquipmentItem> ringsInventory;
        public List<ConsumableItem> consumablesInventory;

        private void Start()
        {
            DataManager dataManager = SaveManager.LoadGame();

            if(dataManager != null)
            {
                for (int i = 0; i < SettingsHolder.partsID.Length; i++)
                {
                    SettingsHolder.partsID[i] = dataManager.partsID[i];
                    SettingsHolder.partsArmor[i] = dataManager.partsArmor[i];
                }

                if (!dataManager.isFirstStart)
                {

                }
            }

            GameObject player_ = GameObject.FindGameObjectWithTag("Player");

            if (player_ != null)
            {
                player_.GetComponent<CurrentEquipments>().InitializeCurrentEquipment();
                player_.GetComponent<CurrentEquipments>().EquipPlayerWithCurrentItems();
                player_.GetComponent<CurrentEquipments>().UpdateArmorValue();
            }
        }
    }
}