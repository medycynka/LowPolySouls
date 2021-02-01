using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.Items.Consumable
{
    public class ConsumablePickUp : Interactable
    {
        [Header("Consumable Item Pick Up", order = 1)]
        [Header("Consumable Items List", order = 2)]
        public ConsumableItem[] consumableItems;

        public override void Interact(PlayerManager playerManager)
        {
            PickUpItem(playerManager);
        }

        public override void PickUpItem(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            if(consumableItems.Length > 0)
            {
                if (consumableItems[0].isDeathDrop)
                {
                    playerManager.GetComponent<PlayerStats>().soulsAmount += consumableItems[0].soulAmount;
                    uIManager.UpdateSouls();
                }
                else
                {
                    foreach (var consumableItem in consumableItems)
                    {
                        if (consumableItem != null)
                        {
                            playerInventory.consumablesInventory.Add(consumableItem);
                        }
                    }

                    uIManager.GetConsumableInventorySlot();
                    uIManager.UpdateConsumableInventory();
                    uIManager.UpdateEstusAmount();
                }

                playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = consumableItems[0].itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = consumableItems[0].itemIcon.texture;
            }

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}