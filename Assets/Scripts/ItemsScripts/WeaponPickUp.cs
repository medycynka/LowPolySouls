using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero; //Stops the player from moving whilst picking up item
            animatorHandler.PlayTargetAnimation("Pick_Up_Item", true); //Plays the animation of looting the item

            if (weapon != null)
            {
                if (weapon.itemType == ItemType.Weapon)
                {
                    playerInventory.weaponsInventory.Add(weapon);
                }
                else if(weapon.itemType == ItemType.Shield)
                {
                    playerInventory.weaponsInventory.Add(weapon);
                }

                playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            }
            
            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}
