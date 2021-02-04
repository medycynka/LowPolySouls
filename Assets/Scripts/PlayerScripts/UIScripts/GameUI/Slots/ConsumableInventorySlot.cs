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
        public PlayerAnimatorHandler playerAnimatorHandler;

        private ConsumableItem _item;

        public void AddItem(ConsumableItem newItem)
        {
            _item = newItem;
            icon.sprite = _item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot(bool lastSlot)
        {
            _item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(lastSlot);
        }

        public void UseThisItem()
        {
            if (_item != null)
            {
                switch (_item.consumableType)
                {
                    case ConsumableType.HealItem:
                        playerStats.healthRefillAmount = _item.healAmount;
                        playerManager.shouldRefillHealth = true;
                        playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.EstusName], true);
                        break;
                    case ConsumableType.SoulItem:
                        playerStats.soulsAmount += _item.soulAmount;
                        playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.UseItemName], true);
                        break;
                    case ConsumableType.ManaItem:
                        break;
                }

                playerInventory.consumablesInventory.Remove(_item);
                uiManager.GetConsumableInventorySlot();
                uiManager.UpdateConsumableInventory();
                uiManager.UpdateEstusAmount();
                uiManager.UpdateSouls();
            }
        }
    }
}