using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    [CreateAssetMenu(menuName = "Items/Consumable Item")]
    public class ConsumableItem : Item
    {
        public ConsumableType consumableType;

        public float healAmount;
        public float soulAmount;
        public float manaAmount;
    }

}