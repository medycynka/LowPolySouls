using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class EnemyStats : CharacterStats
    {
        public Animator animator;

        [Header("Souls & souls target")]
        public float soulsGiveAmount;
        public PlayerStats playerStats;
        public GameObject enemyObject;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth > 0)
            {
                animator.Play("Damage_01");
            }
        }

        public void DealDamage(PlayerStats playerStats, float weaponDamage)
        {
            playerStats.TakeDamage(weaponDamage + Strength);
        }
    }

}
