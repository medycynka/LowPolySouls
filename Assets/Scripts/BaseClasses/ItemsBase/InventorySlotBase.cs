using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class InventorySlotBase : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public WeaponSlotManager weaponSlotManager;
        public UIManager uiManager;

        public Image icon;
    }

}