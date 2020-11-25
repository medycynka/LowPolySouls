using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class EquipmentPickUp : Interactable
    {
        public EquipmentItem equipment;

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

            if (equipment != null)
            {
                switch (equipment.itemType)
                {
                    case ItemType.Helmet:
                        playerInventory.equipmentInventory.Add(equipment);
                        break;
                    case ItemType.ChestArmor:
                        playerInventory.equipmentInventory.Add(equipment);
                        break;
                    case ItemType.ShoulderArmor:
                        playerInventory.equipmentInventory.Add(equipment);
                        break;
                    case ItemType.HandArmor:
                        playerInventory.equipmentInventory.Add(equipment);
                        break;
                    case ItemType.LegArmor:
                        playerInventory.equipmentInventory.Add(equipment);
                        break;
                    case ItemType.FootArmor:
                        playerInventory.equipmentInventory.Add(equipment);
                        break;
                    case ItemType.Ring:
                        playerInventory.equipmentInventory.Add(equipment);
                        break;
                    default:
                        break;
                }
                
                playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = equipment.itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = equipment.itemIcon.texture;
            }

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}
