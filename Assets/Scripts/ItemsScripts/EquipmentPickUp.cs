using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class EquipmentPickUp : Interactable
    {
        public EquipmentItem[] equipments;

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

            if (equipments.Length > 0)
            {
                foreach (var equipment in equipments)
                {
                    if (equipment != null)
                    {
                        switch (equipment.itemType)
                        {
                            case ItemType.Helmet:
                                playerInventory.helmetsInventory.Add(equipment);
                                break;
                            case ItemType.ChestArmor:
                                playerInventory.chestsInventory.Add(equipment);
                                break;
                            case ItemType.ShoulderArmor:
                                playerInventory.shouldersInventory.Add(equipment);
                                break;
                            case ItemType.HandArmor:
                                playerInventory.handsInventory.Add(equipment);
                                break;
                            case ItemType.LegArmor:
                                playerInventory.legsInventory.Add(equipment);
                                break;
                            case ItemType.FootArmor:
                                playerInventory.feetInventory.Add(equipment);
                                break;
                            case ItemType.Ring:
                                playerInventory.ringsInventory.Add(equipment);
                                break;
                            default:
                                break;
                        }
                    }
                }
                
                playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = equipments[0].itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = equipments[0].itemIcon.texture;
            }

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}
