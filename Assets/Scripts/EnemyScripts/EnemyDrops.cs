using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class EnemyDrops : MonoBehaviour
    {
        [Header("Drop chances")]
        public float consumableChanse = 0.45f;
        public float weaponChanse = 0.25f;
        public float equipmentChanse = 0.2f;

        [Header("Drops pool")]
        public ConsumableItem[] consumableDropPool;
        public WeaponItem[] weaponDropPool;
        public EquipmentItem[] equipmentDropPool;

        [SerializeField] List<Item> dropList;
        [SerializeField] MiscPickUp deathDrop;
        [SerializeField] float dropChance;

        private void Start()
        {
            dropList = new List<Item>();
            dropChance = Random.Range(0.0f, 1.0f);
            deathDrop.items_.Clear();

            if (dropChance <= consumableChanse)
            {
                // Drop consumable
                foreach (var drop_ in consumableDropPool)
                {
                    dropList.Add(drop_);
                }
            }
            else if (dropChance - consumableChanse <= equipmentChanse)
            {
                // Drop weapon
                foreach (var drop_ in weaponDropPool)
                {
                    dropList.Add(drop_);
                }
            }
            else if (dropChance - consumableChanse - equipmentChanse <= weaponChanse)
            {
                // Drop equipment
                foreach (var drop_ in equipmentDropPool)
                {
                    dropList.Add(drop_);
                }
            }
            else
            {
                // Drop all
                foreach (var drop_ in consumableDropPool)
                {
                    dropList.Add(drop_);
                }

                foreach (var drop_ in weaponDropPool)
                {
                    dropList.Add(drop_);
                }

                foreach (var drop_ in equipmentDropPool)
                {
                    dropList.Add(drop_);
                }
            }
        }

        public void DropPickUp()
        {
            deathDrop.items_ = dropList;
            Instantiate(deathDrop, transform.position, Quaternion.identity);
            deathDrop.items_.Clear();
        }
    }

}