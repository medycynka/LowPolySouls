using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class ConsumableInventorySlot : InventorySlotBase
    {
        public PlayerStats playerStats;
        public PlayerManager playerManager;
        public AnimatorHandler animatorHandler;

        private ConsumableItem item;

        public void AddItem(ConsumableItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot(bool lastSlot)
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(lastSlot);
        }

        public void UseThisItem()
        {
            if (item != null)
            {
                switch (item.consumableType)
                {
                    case ConsumableType.HealItem:
                        playerStats.healthRefillAmount = item.healAmount;
                        playerManager.shouldRefillHealth = true;
                        animatorHandler.PlayTargetAnimation("Use Estus", true);
                        break;
                    case ConsumableType.SoulItem:
                        playerStats.soulsAmount += item.soulAmount;
                        animatorHandler.PlayTargetAnimation("Use Item", true);
                        break;
                    case ConsumableType.ManaItem:
                        break;
                    default:
                        break;
                }

                playerInventory.consumablesInventory.Remove(item);
                uiManager.GetConsumableInventorySlot();
                uiManager.UpdateConsumableInventory();
                uiManager.UpdateEstusAmount();
                uiManager.UpdateSouls();
            }
        }
    }

}