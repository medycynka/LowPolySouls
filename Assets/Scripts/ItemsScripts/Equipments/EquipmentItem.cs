using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleDrakeStudios;
using BattleDrakeStudios.ModularCharacters;

namespace SP
{
    [CreateAssetMenu(menuName = "Items/Equipment Item")]
    public class EquipmentItem : Item
    {
        public ModularBodyPart[] bodyParts;
        public int[] partsIDS;
        public bool removeHead;
        public bool removeHeadFeatures;
        public bool canDeactivate;

        [Header("Stats")]
        public float Armor;
        public int Weight;
        public int Durability;
    }

}
