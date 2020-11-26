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

            public EquipmentPart(ModularBodyPart mbp_, int id_)
            {
                eqPart = mbp_;
                partID = id_;
                armorValue = 0;
            }
        }

        public class RingPart
        {
            public ItemType ring_ = ItemType.Ring;
            public int id_ = -1;
            public int slotID = 0;
            public int armorValue = 0;
        }

        public Dictionary<ModularBodyPart, EquipmentPart> currentEq;
        public RingPart[] currentRings = { new RingPart(), new RingPart(), new RingPart()};

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
                { ModularBodyPart.Helmet, new EquipmentPart(ModularBodyPart.Helmet, -1) },
                { ModularBodyPart.HeadAttachment, new EquipmentPart(ModularBodyPart.HeadAttachment, -1) },
                { ModularBodyPart.Head, new EquipmentPart(ModularBodyPart.Head, 0) },
                { ModularBodyPart.Hat, new EquipmentPart(ModularBodyPart.Hat, -1) },
                { ModularBodyPart.HeadCovering, new EquipmentPart(ModularBodyPart.HeadCovering, -1) },
                { ModularBodyPart.Hair, new EquipmentPart(ModularBodyPart.Hair, 0) },
                { ModularBodyPart.Eyebrow, new EquipmentPart(ModularBodyPart.Eyebrow, 7) },
                { ModularBodyPart.Ear, new EquipmentPart(ModularBodyPart.Ear, 1) },
                { ModularBodyPart.FacialHair, new EquipmentPart(ModularBodyPart.FacialHair, 0) },
                { ModularBodyPart.BackAttachment, new EquipmentPart(ModularBodyPart.BackAttachment, -1) },
                { ModularBodyPart.Torso, new EquipmentPart(ModularBodyPart.Torso, 0) },
                { ModularBodyPart.ShoulderAttachmentRight, new EquipmentPart(ModularBodyPart.ShoulderAttachmentRight, -1) },
                { ModularBodyPart.ShoulderAttachmentLeft, new EquipmentPart(ModularBodyPart.ShoulderAttachmentLeft, -1) },
                { ModularBodyPart.ArmUpperRight, new EquipmentPart(ModularBodyPart.ArmUpperRight, 0) },
                { ModularBodyPart.ArmUpperLeft, new EquipmentPart(ModularBodyPart.ArmUpperLeft, 0) },
                { ModularBodyPart.ElbowAttachmentRight, new EquipmentPart(ModularBodyPart.ElbowAttachmentRight, -1) },
                { ModularBodyPart.ElbowAttachmentLeft, new EquipmentPart(ModularBodyPart.ElbowAttachmentLeft, -1) },
                { ModularBodyPart.ArmLowerRight, new EquipmentPart(ModularBodyPart.ArmLowerRight, 0) },
                { ModularBodyPart.ArmLowerLeft, new EquipmentPart(ModularBodyPart.ArmLowerLeft, 0) },
                { ModularBodyPart.HandRight, new EquipmentPart(ModularBodyPart.HandRight, 0) },
                { ModularBodyPart.HandLeft, new EquipmentPart(ModularBodyPart.HandLeft, 0) },
                { ModularBodyPart.Hips, new EquipmentPart(ModularBodyPart.Hips, 0) },
                { ModularBodyPart.KneeAttachmentRight, new EquipmentPart(ModularBodyPart.KneeAttachmentRight, -1) },
                { ModularBodyPart.KneeAttachmentLeft, new EquipmentPart(ModularBodyPart.KneeAttachmentLeft, -1) },
                { ModularBodyPart.LegRight, new EquipmentPart(ModularBodyPart.LegRight, 0) },
                { ModularBodyPart.LegLeft, new EquipmentPart(ModularBodyPart.LegLeft, 0) }
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
            playerStats.currentArmorValue = CalculateArmorOfCurrentEquipment();
        }

        private float CalculateArmorOfCurrentEquipment()
        {
            float sum_ = 0.0f;

            foreach(var kvp_ in currentEq)
            {
                sum_ += kvp_.Value.armorValue;
            }

            foreach(var ring_ in currentRings)
            {
                sum_ += ring_.armorValue;
            }

            return sum_;
        }
    }

}