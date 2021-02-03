using UnityEngine;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.EnemyScripts;


namespace SzymonPeszek.Damage
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage Collider", order = 0)]
        [SerializeField] private Collider damageCollider;

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

        public void DisableDamageCollider()
        {
            if (damageCollider != null)
            {
                damageCollider.enabled = false;
            }
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

            if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
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