using SzymonPeszek.PlayerScripts;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Enums;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Misc;

    
namespace SzymonPeszek.GameUI.Slots
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
                        animatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.EstusName], true);
                        break;
                    case ConsumableType.SoulItem:
                        playerStats.soulsAmount += item.soulAmount;
                        animatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.UseItemName], true);
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