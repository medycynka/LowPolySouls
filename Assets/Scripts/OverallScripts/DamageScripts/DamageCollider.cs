using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage Collider", order = 0)]
        Collider damageCollider;

        [Header("Weapon Damage", order = 1)]
        public float currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisaleDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                EnemyStats enemyStats = GetComponentInParent<EnemyStats>();

                if (playerStats != null && enemyStats != null)
                {
                    enemyStats.DealDamage(playerStats, currentWeaponDamage);
                }
            }

            if (collision.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                PlayerStats playerStats = GetComponentInParent<PlayerStats>();

                if (enemyStats != null && playerStats != null)
                {
                    playerStats.DealDamage(enemyStats, currentWeaponDamage);
                }
            }
        }
    }

}