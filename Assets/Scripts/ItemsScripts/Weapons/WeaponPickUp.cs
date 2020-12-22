using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class WeaponPickUp : Interactable
    {
        [Header("Consumable Item Pick Up", order = 1)]
        [Header("Consumable Items List", order = 2)]
        public WeaponItem[] weapons;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        public override void PickUpItem(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            if (weapons.Length > 0)
            {
                foreach (var weapon in weapons)
                {
                    if (weapon != null)
                    {
                        if (weapon.itemType == ItemType.Weapon)
                        {
                            playerInventory.weaponsInventory.Add(weapon);
                            uIManager.GetWeaponInventorySlot();
                            uIManager.UpdateWeaponInventory();
                        }
                        else if (weapon.itemType == ItemType.Shield)
                        {
                            playerInventory.shieldsInventory.Add(weapon);
                            uIManager.GetShieldInventorySlot();
                            uIManager.UpdateShieldInventory();
                        }
                    }
                }

                playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapons[0].itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapons[0].itemIcon.texture;
            }
            
            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}
