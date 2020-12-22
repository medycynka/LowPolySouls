using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class InventorySlotBase : MonoBehaviour
    {
        [Header("Inventory Slot", order = 0)]
        [Header("Inventory Slot Basic Components", order = 1)]
        public PlayerInventory playerInventory;
        public WeaponSlotManager weaponSlotManager;
        public UIManager uiManager;

        [Header("Inventory Slot Icon", order = 1)]
        public Image icon;
    }

}