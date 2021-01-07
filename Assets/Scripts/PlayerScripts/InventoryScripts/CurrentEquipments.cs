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
            public int dictID;
            public ModularBodyPart eqPart;
            public int partID;
            public float armorValue;

            public EquipmentPart(int did_, ModularBodyPart mbp_, int id_, float armor_)
            {
                dictID = did_;
                eqPart = mbp_;
                partID = id_;
                armorValue = armor_;
            }
        }

        public Dictionary<ModularBodyPart, EquipmentPart> currentEq;

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
            modularCharacterManager = GetComponentInChildren<ModularCharacterManager>();
        }

        private void Start()
        {
            if (SettingsHolder.isMale)
            {
                modularCharacterManager.SwapGender(Gender.Male);
            }
            else
            {
                modularCharacterManager.SwapGender(Gender.Female);
            }

            InitializeCurrentEquipment();
            EquipPlayerWithCurrentItems();
            UpdateArmorValue();
        }

        public void InitializeCurrentEquipment()
        {
            currentEq = new Dictionary<ModularBodyPart, EquipmentPart>(){
                { ModularBodyPart.Helmet, new EquipmentPart(0, ModularBodyPart.Helmet, SettingsHolder.partsID[0], SettingsHolder.partsArmor[0]) },
                { ModularBodyPart.HeadAttachment, new EquipmentPart(1, ModularBodyPart.HeadAttachment, SettingsHolder.partsID[1], SettingsHolder.partsArmor[1]) },
                { ModularBodyPart.Head, new EquipmentPart(2, ModularBodyPart.Head, SettingsHolder.partsID[2], SettingsHolder.partsArmor[2]) },
                { ModularBodyPart.Hat, new EquipmentPart(3, ModularBodyPart.Hat, SettingsHolder.partsID[3], SettingsHolder.partsArmor[3]) },
                { ModularBodyPart.HeadCovering, new EquipmentPart(4, ModularBodyPart.HeadCovering, SettingsHolder.partsID[4], SettingsHolder.partsArmor[4]) },
                { ModularBodyPart.Hair, new EquipmentPart(5, ModularBodyPart.Hair, SettingsHolder.partsID[5], SettingsHolder.partsArmor[5]) },
                { ModularBodyPart.Eyebrow, new EquipmentPart(6, ModularBodyPart.Eyebrow, SettingsHolder.partsID[6], SettingsHolder.partsArmor[6]) },
                { ModularBodyPart.Ear, new EquipmentPart(7, ModularBodyPart.Ear, SettingsHolder.partsID[7], SettingsHolder.partsArmor[7]) },
                { ModularBodyPart.FacialHair, new EquipmentPart(8, ModularBodyPart.FacialHair, SettingsHolder.partsID[8], SettingsHolder.partsArmor[8]) },
                { ModularBodyPart.BackAttachment, new EquipmentPart(9, ModularBodyPart.BackAttachment, SettingsHolder.partsID[9], SettingsHolder.partsArmor[9]) },
                { ModularBodyPart.Torso, new EquipmentPart(10, ModularBodyPart.Torso, SettingsHolder.partsID[10], SettingsHolder.partsArmor[10]) },
                { ModularBodyPart.ShoulderAttachmentRight, new EquipmentPart(11, ModularBodyPart.ShoulderAttachmentRight, SettingsHolder.partsID[11], SettingsHolder.partsArmor[11]) },
                { ModularBodyPart.ShoulderAttachmentLeft, new EquipmentPart(12, ModularBodyPart.ShoulderAttachmentLeft, SettingsHolder.partsID[12], SettingsHolder.partsArmor[12]) },
                { ModularBodyPart.ArmUpperRight, new EquipmentPart(13, ModularBodyPart.ArmUpperRight, SettingsHolder.partsID[13], SettingsHolder.partsArmor[13]) },
                { ModularBodyPart.ArmUpperLeft, new EquipmentPart(14, ModularBodyPart.ArmUpperLeft, SettingsHolder.partsID[14], SettingsHolder.partsArmor[14]) },
                { ModularBodyPart.ElbowAttachmentRight, new EquipmentPart(15, ModularBodyPart.ElbowAttachmentRight, SettingsHolder.partsID[15], SettingsHolder.partsArmor[15]) },
                { ModularBodyPart.ElbowAttachmentLeft, new EquipmentPart(16, ModularBodyPart.ElbowAttachmentLeft, SettingsHolder.partsID[16], SettingsHolder.partsArmor[16]) },
                { ModularBodyPart.ArmLowerRight, new EquipmentPart(17, ModularBodyPart.ArmLowerRight, SettingsHolder.partsID[17], SettingsHolder.partsArmor[17]) },
                { ModularBodyPart.ArmLowerLeft, new EquipmentPart(18, ModularBodyPart.ArmLowerLeft, SettingsHolder.partsID[18], SettingsHolder.partsArmor[18]) },
                { ModularBodyPart.HandRight, new EquipmentPart(19, ModularBodyPart.HandRight, SettingsHolder.partsID[19], SettingsHolder.partsArmor[19]) },
                { ModularBodyPart.HandLeft, new EquipmentPart(20, ModularBodyPart.HandLeft, SettingsHolder.partsID[20], SettingsHolder.partsArmor[20]) },
                { ModularBodyPart.Hips, new EquipmentPart(21, ModularBodyPart.Hips, SettingsHolder.partsID[21], SettingsHolder.partsArmor[21]) },
                { ModularBodyPart.KneeAttachmentRight, new EquipmentPart(22, ModularBodyPart.KneeAttachmentRight, SettingsHolder.partsID[22], SettingsHolder.partsArmor[22]) },
                { ModularBodyPart.KneeAttachmentLeft, new EquipmentPart(23, ModularBodyPart.KneeAttachmentLeft, SettingsHolder.partsID[23], SettingsHolder.partsArmor[23]) },
                { ModularBodyPart.LegRight, new EquipmentPart(24, ModularBodyPart.LegRight, SettingsHolder.partsID[24], SettingsHolder.partsArmor[24]) },
                { ModularBodyPart.LegLeft, new EquipmentPart(25, ModularBodyPart.LegLeft, SettingsHolder.partsID[25], SettingsHolder.partsArmor[25]) }
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

        public void SaveCurrentEqIds()
        {
            foreach (var kvp_ in currentEq)
            {
                SettingsHolder.partsID[kvp_.Value.dictID] = kvp_.Value.partID;
                SettingsHolder.partsArmor[kvp_.Value.dictID] = kvp_.Value.armorValue;
            }
        }

        public void UpdateArmorValue()
        {
            playerStats.currentArmorValue = playerStats.baseArmor + CalculateArmorOfCurrentEquipment() + 2.5f * playerStats.Defence + 0.5f * playerStats.Agility;
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