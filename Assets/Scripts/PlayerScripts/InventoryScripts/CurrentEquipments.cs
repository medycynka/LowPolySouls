using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleDrakeStudios;
using BattleDrakeStudios.ModularCharacters;

namespace SP
{

    public class CurrentEquipments : MonoBehaviour
    {
        PlayerStats playerStats;
        ModularCharacterManager modularCharacterManager;

        public class EquipmentPart
        {
            public ModularBodyPart eqPart;
            public int partID;
            public float armorValue;

            public EquipmentPart(ModularBodyPart mbp_, int id_, float armor_)
            {
                eqPart = mbp_;
                partID = id_;
                armorValue = armor_;
            }
        }

        public Dictionary<ModularBodyPart, EquipmentPart> currentEq;

        private void Start()
        {
            playerStats = GetComponent<PlayerStats>();
            modularCharacterManager = GetComponentInChildren<ModularCharacterManager>();

            InitializeCurrentEquipment();
            EquipPlayerWithCurrentItems();
            UpdateArmorValue();
        }

        private void InitializeCurrentEquipment()
        {
            currentEq = new Dictionary<ModularBodyPart, EquipmentPart>(){
                { ModularBodyPart.Helmet, new EquipmentPart(ModularBodyPart.Helmet, -1, 0) },
                { ModularBodyPart.HeadAttachment, new EquipmentPart(ModularBodyPart.HeadAttachment, -1, 0) },
                { ModularBodyPart.Head, new EquipmentPart(ModularBodyPart.Head, 0, 0) },
                { ModularBodyPart.Hat, new EquipmentPart(ModularBodyPart.Hat, -1, 0) },
                { ModularBodyPart.HeadCovering, new EquipmentPart(ModularBodyPart.HeadCovering, -1, 0) },
                { ModularBodyPart.Hair, new EquipmentPart(ModularBodyPart.Hair, 0, 0) },
                { ModularBodyPart.Eyebrow, new EquipmentPart(ModularBodyPart.Eyebrow, 7, 0) },
                { ModularBodyPart.Ear, new EquipmentPart(ModularBodyPart.Ear, 1, 0) },
                { ModularBodyPart.FacialHair, new EquipmentPart(ModularBodyPart.FacialHair, 0, 0) },
                { ModularBodyPart.BackAttachment, new EquipmentPart(ModularBodyPart.BackAttachment, -1, 0) },
                { ModularBodyPart.Torso, new EquipmentPart(ModularBodyPart.Torso, 0, 0) },
                { ModularBodyPart.ShoulderAttachmentRight, new EquipmentPart(ModularBodyPart.ShoulderAttachmentRight, -1, 0) },
                { ModularBodyPart.ShoulderAttachmentLeft, new EquipmentPart(ModularBodyPart.ShoulderAttachmentLeft, -1, 0) },
                { ModularBodyPart.ArmUpperRight, new EquipmentPart(ModularBodyPart.ArmUpperRight, 0, 0) },
                { ModularBodyPart.ArmUpperLeft, new EquipmentPart(ModularBodyPart.ArmUpperLeft, 0, 0) },
                { ModularBodyPart.ElbowAttachmentRight, new EquipmentPart(ModularBodyPart.ElbowAttachmentRight, -1, 0) },
                { ModularBodyPart.ElbowAttachmentLeft, new EquipmentPart(ModularBodyPart.ElbowAttachmentLeft, -1, 0) },
                { ModularBodyPart.ArmLowerRight, new EquipmentPart(ModularBodyPart.ArmLowerRight, 0, 0) },
                { ModularBodyPart.ArmLowerLeft, new EquipmentPart(ModularBodyPart.ArmLowerLeft, 0, 0) },
                { ModularBodyPart.HandRight, new EquipmentPart(ModularBodyPart.HandRight, 0, 0) },
                { ModularBodyPart.HandLeft, new EquipmentPart(ModularBodyPart.HandLeft, 0, 0) },
                { ModularBodyPart.Hips, new EquipmentPart(ModularBodyPart.Hips, 0, 0) },
                { ModularBodyPart.KneeAttachmentRight, new EquipmentPart(ModularBodyPart.KneeAttachmentRight, -1, 0) },
                { ModularBodyPart.KneeAttachmentLeft, new EquipmentPart(ModularBodyPart.KneeAttachmentLeft, -1, 0) },
                { ModularBodyPart.LegRight, new EquipmentPart(ModularBodyPart.LegRight, 0, 0) },
                { ModularBodyPart.LegLeft, new EquipmentPart(ModularBodyPart.LegLeft, 0, 0) }
            };
        }

        public void EquipPlayerWithCurrentItems()
        {
            foreach(var kvp_ in currentEq)
            {
                if(kvp_.Value.partID > -1)
                {
                    modularCharacterManager.ActivatePart(kvp_.Value.eqPart, kvp_.Value.partID);
                }
            }
        }

        public void UpdateArmorValue()
        {
            playerStats.currentArmorValue = playerStats.baseArmor + CalculateArmorOfCurrentEquipment();
        }

        private float CalculateArmorOfCurrentEquipment()
        {
            float sum_ = 0.0f;

            foreach(var kvp_ in currentEq)
            {
                sum_ += kvp_.Value.armorValue;
            }

            return sum_;
        }
    }

}