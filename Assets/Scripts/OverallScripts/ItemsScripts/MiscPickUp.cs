using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SP
{

    public class MiscPickUp : Interactable
    {
        public List<Item> items_;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        public override void PickUpItem(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            if (items_.Count > 0)
            {
                foreach(var item_ in items_)
                {
                    if(item_ != null)
                    {
                        if(item_ is WeaponItem)
                        {
                            if (item_.itemType == ItemType.Weapon)
                            {
                                playerInventory.weaponsInventory.Add((WeaponItem)item_);
                                uIManager.GetWeaponInventorySlot();
                                uIManager.UpdateWeaponInventory();
                            }
                            else if (item_.itemType == ItemType.Shield)
                            {
                                playerInventory.shieldsInventory.Add((WeaponItem)item_);
                                uIManager.GetShieldInventorySlot();
                                uIManager.UpdateShieldInventory();
                            }
                        }
                        else if(item_ is EquipmentItem)
                        {
                            switch (item_.itemType)
                            {
                                case ItemType.Helmet:
                                    playerInventory.helmetsInventory.Add((EquipmentItem)item_);
                                    uIManager.GetHelmetInventorySlot();
                                    uIManager.UpdateHelmetInventory();
                                    break;
                                case ItemType.ChestArmor:
                                    playerInventory.chestsInventory.Add((EquipmentItem)item_);
                                    uIManager.GetChestInventorySlot();
                                    uIManager.UpdateChestInventory();
                                    break;
                                case ItemType.ShoulderArmor:
                                    playerInventory.shouldersInventory.Add((EquipmentItem)item_);
                                    uIManager.GetShoulderInventorySlot();
                                    uIManager.UpdateShoulderInventory();
                                    break;
                                case ItemType.HandArmor:
                                    playerInventory.handsInventory.Add((EquipmentItem)item_);
                                    uIManager.GetHandInventorySlot();
                                    uIManager.UpdateHandInventory();
                                    break;
                                case ItemType.LegArmor:
                                    playerInventory.legsInventory.Add((EquipmentItem)item_);
                                    uIManager.GetLegInventorySlot();
                                    uIManager.UpdateLegInventory();
                                    break;
                                case ItemType.FootArmor:
                                    playerInventory.feetInventory.Add((EquipmentItem)item_);
                                    uIManager.GetFootInventorySlot();
                                    uIManager.UpdateFootInventory();
                                    break;
                                case ItemType.Ring:
                                    playerInventory.ringsInventory.Add((EquipmentItem)item_);
                                    uIManager.GetRingInventorySlot();
                                    uIManager.UpdateRingInventory();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if(item_ is ConsumableItem)
                        {
                            playerInventory.consumablesInventory.Add((ConsumableItem)item_);
                            uIManager.GetConsumableInventorySlot();
                            uIManager.UpdateConsumableInventory();
                        }
                    }
                }

                playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = items_[0].itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = items_[0].itemIcon.texture;
                uIManager.UpdateEstusAmount();
            }

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}