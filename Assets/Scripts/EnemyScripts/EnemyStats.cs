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
        EnemyManager enemyManager;

        [Header("Enemy Properties", order = 1)]
        [Header("Animator", order = 2)]
        public Animator animator;

        [Header("Health Bar", order = 2)]
        public GameObject healthBar;
        public Image healtBarFill;
        public TextMeshProUGUI damageValue;

        [Header("Souls & souls target", order = 2)]
        public float soulsGiveAmount;
        public PlayerStats playerStats;
        public GameObject enemyObject;

        [Header("Is it Boss", order = 2)]
        public bool isBoss;
        public BossAreaManager bossAreaManager;
        Slider bossHpSlider;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            animator = GetComponentInChildren<Animator>();
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }

        void Start()
        {
            InitializeHealth();
        }

        private void LateUpdate()
        {
            if (!isBoss)
            {
                healthBar.transform.LookAt(Camera.main.transform);
                healthBar.transform.Rotate(0, 180, 0);
            }
        }

        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void InitializeHealth()
        {
            if (isBoss)
            {
                maxHealth = SetMaxHealthFromHealthLevel();
                currentHealth = maxHealth;

                if (bossHpSlider == null)
                {
                    bossHpSlider = bossAreaManager.bossHpBar.GetComponentInChildren<Slider>();
                }

                bossHpSlider.maxValue = maxHealth;
                bossHpSlider.value = currentHealth;
            }
            else
            {
                maxHealth = SetMaxHealthFromHealthLevel();
                currentHealth = maxHealth;
                healtBarFill.fillAmount = 1f;
                healthBar.SetActive(false);
            }
        }

        public void TakeDamage(float damage)
        {
            if (enemyManager.isAlive)
            {
                if (isBoss)
                {
                    currentHealth -= damage;
                    bossHpSlider.value = currentHealth;

                    if (currentHealth > 0)
                    {
                        animator.Play("Damage_01");
                    }

                    if(currentHealth <= 0)
                    {
                        bossAreaManager.bossHpBar.SetActive(false);
                    }
                }
                else
                {
                    StartCoroutine(UpdateEnemyHealthBar(damage));
                }
            }
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
            }

            yield return new WaitForSeconds(4f);

            healthBar.SetActive(false);
        }
    }

}
