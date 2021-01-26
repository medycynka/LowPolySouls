using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleDrakeStudios;
using BattleDrakeStudios.ModularCharacters;

namespace SP
{

    public class EquipmentInventorySlot : InventorySlotBase
    {
        public CurrentEquipments currentEquipments;
        public ModularCharacterManager modularCharacterManager;

        public bool equipUnEquip = false;
        
        private EquipmentItem item;
        private bool shouldDeactivate = false;

        public void AddItem(EquipmentItem newItem)
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

        public void HandleEquiping()
        {
            if (item != null)
            {
                equipUnEquip = !equipUnEquip;

                if (equipUnEquip)
                {
                    EquipThisItem();

                    switch (item.itemType)
                    {
                        case ItemType.Helmet:
                            uiManager.GetHelmetInventorySlot();
                            uiManager.UpdateHelmetInventory();
                            break;
                        case ItemType.ChestArmor:
                            uiManager.GetChestInventorySlot();
                            uiManager.UpdateChestInventory();
                            break;
                        case ItemType.ShoulderArmor:
                            uiManager.GetShoulderInventorySlot();
                            uiManager.UpdateShoulderInventory();
                            break;
                        case ItemType.HandArmor:
                            uiManager.GetHandInventorySlot();
                            uiManager.UpdateHandInventory();
                            break;
                        case ItemType.LegArmor:
                            uiManager.GetLegInventorySlot();
                            uiManager.UpdateLegInventory();
                            break;
                        case ItemType.FootArmor:
                            uiManager.GetFootInventorySlot();
                            uiManager.UpdateFootInventory();
                            break;
                        case ItemType.Ring:
                            uiManager.GetRingInventorySlot();
                            uiManager.UpdateRingInventory();
                            break;
                        default:
                            break;
                    }

                    equipUnEquip = true;
                }
                else
                {
                    UnequipThisItem();
                }
            }
        }
        private void EquipThisItem()
        {
            if (item.itemType == ItemType.Ring)
            {
                
            }
            else
            {
                #region Deactivate Head if necessary
                if (item.removeHead)
                {
                    modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Head].eqPart);
                }
                #endregion

                int counter = 0;
                foreach (var bodyPart in item.bodyParts)
                {
                    #region Deactivate Unnecesary Head Parts
                    if (item.removeHeadFeatures)
                    {
                        if (bodyPart == ModularBodyPart.Helmet)
                        {
                            if (currentEquipments.CurrentEq[ModularBodyPart.Hat].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Hat].eqPart);
                                currentEquipments.CurrentEq[ModularBodyPart.Hat].partID = -1;
                                currentEquipments.CurrentEq[ModularBodyPart.Hat].armorValue = 0;
                            }
                            if (currentEquipments.CurrentEq[ModularBodyPart.HeadCovering].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.HeadCovering].eqPart);
                                currentEquipments.CurrentEq[ModularBodyPart.HeadCovering].partID = -1;
                                currentEquipments.CurrentEq[ModularBodyPart.HeadCovering].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Ear].eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Hair].eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Eyebrow].eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.FacialHair].eqPart);
                        }
                        else if (bodyPart == ModularBodyPart.HeadCovering)
                        {
                            if (currentEquipments.CurrentEq[ModularBodyPart.Hat].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Hat].eqPart);
                                currentEquipments.CurrentEq[ModularBodyPart.Hat].partID = -1;
                                currentEquipments.CurrentEq[ModularBodyPart.Hat].armorValue = 0;
                            }
                            if (currentEquipments.CurrentEq[ModularBodyPart.Helmet].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Helmet].eqPart);
                                currentEquipments.CurrentEq[ModularBodyPart.Helmet].partID = -1;
                                currentEquipments.CurrentEq[ModularBodyPart.Helmet].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Ear].eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Hair].eqPart);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Head].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Head].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Eyebrow].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.FacialHair].eqPart, currentEquipments.CurrentEq[ModularBodyPart.FacialHair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.Hat)
                        {
                            if (currentEquipments.CurrentEq[ModularBodyPart.Helmet].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Helmet].eqPart);
                                currentEquipments.CurrentEq[ModularBodyPart.Helmet].partID = -1;
                                currentEquipments.CurrentEq[ModularBodyPart.Helmet].armorValue = 0;
                            }
                            if (currentEquipments.CurrentEq[ModularBodyPart.HeadCovering].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.HeadCovering].eqPart);
                                currentEquipments.CurrentEq[ModularBodyPart.HeadCovering].partID = -1;
                                currentEquipments.CurrentEq[ModularBodyPart.HeadCovering].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.CurrentEq[ModularBodyPart.Hair].eqPart);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Head].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Head].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Ear].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Eyebrow].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.FacialHair].eqPart, currentEquipments.CurrentEq[ModularBodyPart.FacialHair].partID);
                        }
                    }
                    #endregion

                    #region Equip Item
                    modularCharacterManager.ActivatePart(bodyPart, item.partsIDS[counter]);
                    currentEquipments.CurrentEq[bodyPart].partID = item.partsIDS[counter];
                    currentEquipments.CurrentEq[bodyPart].armorValue = item.Armor;
                    #endregion

                    counter++;
                }
            }

            currentEquipments.SaveCurrentEqIds();
            currentEquipments.UpdateArmorValue();
        }

        private void UnequipThisItem()
        {
            if (item.itemType == ItemType.Ring)
            {

            }
            else
            {
                #region Activate Head if necessary
                if (item.removeHead)
                {
                    modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Head].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Head].partID);
                }
                #endregion

                foreach (var bodyPart in item.bodyParts)
                {
                    #region Activate Head Parts
                    if (item.removeHeadFeatures)
                    {
                        if (bodyPart == ModularBodyPart.Helmet)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Ear].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Hair].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Hair].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Eyebrow].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.FacialHair].eqPart, currentEquipments.CurrentEq[ModularBodyPart.FacialHair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.HeadCovering)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Ear].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Hair].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Hair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.Hat)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.CurrentEq[ModularBodyPart.Hair].eqPart, currentEquipments.CurrentEq[ModularBodyPart.Hair].partID);
                        }
                    }
                    #endregion

                    #region Unequip Item
                    shouldDeactivate = item.canDeactivate || (bodyPart == ModularBodyPart.ShoulderAttachmentLeft
                        || bodyPart == ModularBodyPart.ShoulderAttachmentRight
                        || bodyPart == ModularBodyPart.ElbowAttachmentRight
                        || bodyPart == ModularBodyPart.ElbowAttachmentLeft
                        || bodyPart == ModularBodyPart.BackAttachment
                        || bodyPart == ModularBodyPart.KneeAttachmentRight
                        || bodyPart == ModularBodyPart.KneeAttachmentLeft);

                    if (shouldDeactivate)
                    {
                        modularCharacterManager.DeactivatePart(bodyPart);
                        currentEquipments.CurrentEq[bodyPart].partID = -1;
                    }
                    else
                    {
                        modularCharacterManager.ActivatePart(bodyPart, 0);
                        currentEquipments.CurrentEq[bodyPart].partID = 0;
                    }

                    currentEquipments.CurrentEq[bodyPart].armorValue = 0;
                    #endregion
                }
            }

            currentEquipments.SaveCurrentEqIds();
            currentEquipments.UpdateArmorValue();
        }
    }

}