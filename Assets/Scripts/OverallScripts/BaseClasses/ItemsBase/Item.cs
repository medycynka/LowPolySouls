using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class Item : ScriptableObject
    {
        [Header("Item",order = 0)]
        [Header("Item Properties",order = 1)]
        public Sprite itemIcon;
        public string itemName;
        public int itemId;
        public ItemType itemType;
    }

}