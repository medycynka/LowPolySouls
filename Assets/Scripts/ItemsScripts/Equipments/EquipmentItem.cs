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
        [Header("Equipment Item", order = 1)]
        [Header("Item Parts", order = 2)]
        public ModularBodyPart[] bodyParts;
        public int[] partsIDS;

        [Header("Bools", order = 2)]
        public bool removeHead;
        public bool removeHeadFeatures;
        public bool canDeactivate;

        [Header("Item Stats", order = 2)]
        public float Armor;
        public int Weight;
        public int Durability;
    }

}
