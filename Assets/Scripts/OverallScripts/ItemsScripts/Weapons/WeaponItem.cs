using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        [Header("Weapon Item", order = 1)]
        [Header("Weapon Prefab", order = 2)]
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Transformations for Left Hand", order = 2)]
        public Vector3 pivotPositionTransform;
        public Vector3 pivotRotationTransform;
        public Vector3 backSlotPosition;
        public Vector3 backSlotRotation;

        [Header("Weapon Stats", order = 2)]
        public int Light_Attack_Damage;
        public int Heavy_Attack_Damage;
        public int Weight;
        public int Durability;

        [Header("Weapon Idle Animations", order = 2)]
        public string Right_Hand_Idle;
        public string Left_Hand_Idle;
        public string TH_Idle;

        [Header("One Handed Attack Animations", order = 2)]
        public string OH_Light_Attack_1 = "OH_Light_Attack_01";
        public string OH_Light_Attack_2 = "OH_Light_Attack_02";
        public string OH_Light_Attack_3 = "OH_Light_Attack_03";
        public string OH_Heavy_Attack_1 = "OH_Heavy_Attack_01";
        public string OH_Heavy_Attack_2 = "OH_Heavy_Attack_02";
        public string TH_Light_Attack_1 = "TH_Light_Attack_01";
        public string TH_Light_Attack_2 = "TH_Light_Attack_02";
        public string TH_Light_Attack_3 = "TH_Light_Attack_03";
        public string TH_Heavy_Attack_1 = "TH_Heavy_Attack_01";
        public string TH_Heavy_Attack_2 = "TH_Heavy_Attack_02";

        [Header("Stamina Costs", order = 2)]
        public int baseStamina;
        public float oh_lightAttackMultiplier;
        public float oh_heavyAttackMultiplier;
        public float th_lightAttackMultiplier;
        public float th_heavyAttackMultiplier;
    }

}
