using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace SP
{
    public class EnemyStats : CharacterStats
    {
        public Animator animator;

        [Header("Health Bar")]
        public GameObject healthBar;
        public Image healtBarFill;
        public TextMeshProUGUI damageValue;

        [Header("Souls & souls target")]
        public float soulsGiveAmount;
        public PlayerStats playerStats;
        public GameObject enemyObject;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healtBarFill.fillAmount = 1f;
            healthBar.SetActive(false);
        }

        private void LateUpdate()
        {
            healthBar.transform.LookAt(Camera.main.transform);
            healthBar.transform.Rotate(0, 180, 0);
        }

        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(float damage)
        {
            StartCoroutine(UpdateEnemyHealthBar(damage));
        }

        public void DealDamage(PlayerStats playerStats, float weaponDamage)
        {
            playerStats.TakeDamage(weaponDamage + Strength);
        }

        public IEnumerator UpdateEnemyHealthBar(float damage)
        {
            healthBar.SetActive(true);
            currentHealth -= damage;
            healtBarFill.fillAmount = currentHealth / maxHealth;
            damageValue.text = damage.ToString();

            if (currentHealth > 0)
            {
                animator.Play("Damage_01");
                healthBar.SetActive(false);
            }

            yield return new WaitForSeconds(4f);

            healthBar.SetActive(false);
        }
    }

}
