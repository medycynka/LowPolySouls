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

        EquipmentItem item;
        public bool equipUnEquip = false;

        bool shouldDeactivate = false;

        private void Awake()
        {

        }

        public void AddItem(EquipmentItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void HandleEquiping()
        {
            equipUnEquip = !equipUnEquip;

            if (equipUnEquip)
            {
                EquipThisItem();
                uiManager.UpdateUI();
                equipUnEquip = true;
            }
            else
            {
                UnequipThisItem();
            }
        }
        public void EquipThisItem()
        {
            if (item.itemType == ItemType.Ring)
            {

            }
            else
            {
                #region Deactivate Head if necessary
                if (item.removeHead)
                {
                    modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Head].eqPart);
                }
                #endregion

                int counter_ = 0;
                foreach (var bodyPart in item.bodyParts)
                {
                    #region Deactivate Unnecesary Head Parts
                    if (item.removeHeadFeatures)
                    {
                        if (bodyPart == ModularBodyPart.Helmet)
                        {
                            if (currentEquipments.currentEq[ModularBodyPart.Hat].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hat].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.Hat].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.Hat].armorValue = 0;
                            }
                            if (currentEquipments.currentEq[ModularBodyPart.HeadCovering].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.HeadCovering].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.HeadCovering].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.HeadCovering].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Ear].eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Eyebrow].eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.FacialHair].eqPart);
                        }
                        else if (bodyPart == ModularBodyPart.HeadCovering)
                        {
                            if (currentEquipments.currentEq[ModularBodyPart.Hat].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hat].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.Hat].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.Hat].armorValue = 0;
                            }
                            if (currentEquipments.currentEq[ModularBodyPart.Helmet].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Helmet].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.Helmet].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.Helmet].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Ear].eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Head].eqPart, currentEquipments.currentEq[ModularBodyPart.Head].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Eyebrow].eqPart, currentEquipments.currentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.FacialHair].eqPart, currentEquipments.currentEq[ModularBodyPart.FacialHair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.Hat)
                        {
                            if (currentEquipments.currentEq[ModularBodyPart.Helmet].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Helmet].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.Helmet].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.Helmet].armorValue = 0;
                            }
                            if (currentEquipments.currentEq[ModularBodyPart.HeadCovering].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.HeadCovering].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.HeadCovering].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.HeadCovering].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Head].eqPart, currentEquipments.currentEq[ModularBodyPart.Head].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Ear].eqPart, currentEquipments.currentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Eyebrow].eqPart, currentEquipments.currentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.FacialHair].eqPart, currentEquipments.currentEq[ModularBodyPart.FacialHair].partID);
                        }
                    }
                    #endregion

                    #region Equip Item
                    modularCharacterManager.ActivatePart(bodyPart, item.partsIDS[counter_]);
                    currentEquipments.currentEq[bodyPart].partID = item.partsIDS[counter_];
                    currentEquipments.currentEq[bodyPart].armorValue = item.Armor;
                    #endregion

                    counter_++;
                }
            }

            currentEquipments.UpdateArmorValue();
        }

        public void UnequipThisItem()
        {
            if (item.itemType == ItemType.Ring)
            {

            }
            else
            {
                #region Activate Head if necessary
                if (item.removeHead)
                {
                    modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Head].eqPart, currentEquipments.currentEq[ModularBodyPart.Head].partID);
                }
                #endregion

                foreach (var bodyPart in item.bodyParts)
                {
                    #region Activate Head Parts
                    if (item.removeHeadFeatures)
                    {
                        if (bodyPart == ModularBodyPart.Helmet)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Ear].eqPart, currentEquipments.currentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart, currentEquipments.currentEq[ModularBodyPart.Hair].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Eyebrow].eqPart, currentEquipments.currentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.FacialHair].eqPart, currentEquipments.currentEq[ModularBodyPart.FacialHair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.HeadCovering)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Ear].eqPart, currentEquipments.currentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart, currentEquipments.currentEq[ModularBodyPart.Hair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.Hat)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart, currentEquipments.currentEq[ModularBodyPart.Hair].partID);
                        }
                    }
                    #endregion

                    #region Unequip Item
                    shouldDeactivate = item.canDeactivate || (bodyPart == ModularBodyPart.ElbowAttachmentRight
                        || bodyPart == ModularBodyPart.ElbowAttachmentLeft
                        || bodyPart == ModularBodyPart.BackAttachment
                        || bodyPart == ModularBodyPart.KneeAttachmentRight
                        || bodyPart == ModularBodyPart.KneeAttachmentLeft);

                    if (shouldDeactivate)
                    {
                        modularCharacterManager.DeactivatePart(bodyPart);
                        currentEquipments.currentEq[bodyPart].partID = -1;
                    }
                    else
                    {
                        modularCharacterManager.ActivatePart(bodyPart, 0);
                        currentEquipments.currentEq[bodyPart].partID = 0;
                    }

                    currentEquipments.currentEq[bodyPart].armorValue = 0;
                    #endregion
                }
            }

            currentEquipments.UpdateArmorValue();
        }
    }

}