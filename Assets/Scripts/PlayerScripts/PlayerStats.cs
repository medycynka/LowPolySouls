using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;
        public UIManager uiManager;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public GameObject youDiedLogo;

        public int playerLevel = 12;
        public float soulsAmount = 0;
        public float currentArmorValue = 0;

        [Header("Health & Stamina refill values")]
        public float healthRefillAmount = 20f;
        public float healthBgRefillAmount = 20f;
        public float staminaRefillAmount = 20f;

        public bool isPlayerAlive = true;

        AnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            UpdateHealthBar(SetMaxHealthFromHealthLevel());
            UpdateStaminaBar(SetMaxStaminaFromStaminaLevel());

            youDiedLogo.SetActive(false);
        }

        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10 + bonusHealth;

            return maxHealth;
        }

        public void UpdateHealthBar(float newHealth)
        {
            maxHealth = newHealth;
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10 + bonusStamina;

            return maxStamina;
        }

        public void UpdateStaminaBar(float newStamina)
        {
            maxStamina = newStamina;
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void TakeDamage(float damage)
        {
            playerManager.shouldRefillHealth = false;
            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if (currentHealth <= 0)
            {
                HandleDeathAndRespawn();
            }
        }

        public void RefillHealth()
        {
            currentHealth += healthRefillAmount * Time.deltaTime;

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.healthBarSlider.value += healthRefillAmount * Time.deltaTime;
            healthBar.backgroundSlider.value += healthRefillAmount * Time.deltaTime;
        }

        public void TakeStaminaDamage(float drain)
        {
            currentStamina = currentStamina - drain;

            if(currentStamina < 0)
            {
                currentStamina = 0;
            }

            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RefillStamina()
        {
            currentStamina += staminaRefillAmount * Time.deltaTime;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            staminaBar.staminaBarSlider.value += staminaRefillAmount * Time.deltaTime;
        }

        public void DealDamage(EnemyStats enemyStats, float weaponDamage)
        {
            enemyStats.TakeDamage(weaponDamage + Strength);
        }

        public int CalculateSoulsCost(int level)
        {
            return (int)(0.02f * level * level * level + 3.06f * level * level + 105.6f * level - 895f);
        }

        public void HandleDeathAndRespawn()
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Dead_01", true);
            isPlayerAlive = false;

            StartCoroutine(Respawn());
        }

        public IEnumerator Respawn()
        {
            youDiedLogo.SetActive(true);
            
            yield return new WaitForSeconds(5f);

            youDiedLogo.SetActive(false);
            playerManager.quickMoveScreen.SetActive(true);
            animatorHandler.PlayTargetAnimation("Empty", false);
            UpdateHealthBar(maxHealth);
            UpdateStaminaBar(maxStamina);
            transform.position = playerManager.currentSpawnPoint.transform.position;
            transform.rotation = playerManager.currentSpawnPoint.transform.rotation;
            // Respawn enemis and refresh boss health if alive

            yield return new WaitForSeconds(3f);

            isPlayerAlive = true;
            playerManager.quickMoveScreen.SetActive(false);
        }
    }

}