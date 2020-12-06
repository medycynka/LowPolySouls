using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class ConsumablePickUp : Interactable

    {
        public ConsumableItem consumableItem;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
            PlayerLocomotion playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            AnimatorHandler animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
            UIManager uIManager = playerManager.GetComponent<InputHandler>().uiManager;

            playerLocomotion.rigidbody.velocity = Vector3.zero; //Stops the player from moving whilst picking up item
            animatorHandler.PlayTargetAnimation("Pick_Up_Item", true); //Plays the animation of looting the item

            if(consumableItem != null)
            {
                playerInventory.consumablesInventory.Add(consumableItem);
                playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = consumableItem.itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = consumableItem.itemIcon.texture;
                uIManager.GetConsumableInventorySlot();
                uIManager.UpdateConsumableInventory();
            }

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}