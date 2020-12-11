using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class ConsumablePickUp : Interactable

    {
        public ConsumableItem[] consumableItems;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        public override void PickUpItem(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            if(consumableItems.Length > 0)
            {
                foreach (var consumableItem in consumableItems)
                {
                    if (consumableItem != null)
                    {
                        playerInventory.consumablesInventory.Add(consumableItem);
                        uIManager.GetConsumableInventorySlot();
                        uIManager.UpdateConsumableInventory();
                    }
                }

                playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = consumableItems[0].itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = consumableItems[0].itemIcon.texture;
            }

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}