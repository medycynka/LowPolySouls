using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;

        [Header("Player Properties", order = 1)]

        [Header("UI Compontents", order = 2)]
        public UIManager uiManager;
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public GameObject youDiedLogo;

        [Header("Death Drop", order = 2)]
        public Sprite deathDropIcon;
        public ConsumablePickUp soulDeathDrop;

        [Header("Unique Player Stats", order = 2)]
        public string playerName = SettingsHolder.playerName;
        public int playerLevel = 12;
        public float soulsAmount = 0;
        public float currentArmorValue = 0;

        [Header("Health & Stamina refill values", order = 2)]
        public float healthRefillAmount = 20f;
        public float healthBgRefillAmount = 20f;
        public float staminaRefillAmount = 20f;

        [Header("Bools", order = 2)]
        public bool isPlayerAlive = true;

        AnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();

            DataManager dataManager = SaveManager.LoadGame();

            if(dataManager != null)
            {
                if (!dataManager.isFirstStart)
                {
                    currentHealth = dataManager.currentHealth;
                    currentStamina = dataManager.currentStamina;
                    baseArmor = dataManager.baseArmor;
                    Strength = dataManager.Strength;
                    Agility = dataManager.Agility;
                    Defence = dataManager.Defence;
                    bonusHealth = dataManager.bonusHealth;
                    bonusStamina = dataManager.bonusStamina;
                    playerLevel = dataManager.playerLevel;
                    soulsAmount = dataManager.soulsAmount;
                
                    gameObject.transform.position = new Vector3(dataManager.spawnPointPosition[0], dataManager.spawnPointPosition[1], dataManager.spawnPointPosition[2]);
                    gameObject.transform.rotation = Quaternion.Euler(dataManager.spawnPointRotation[0], dataManager.spawnPointRotation[1], dataManager.spawnPointRotation[2]);
                    playerManager.currentSpawnPoint.transform.position = new Vector3(dataManager.spawnPointPosition[0], dataManager.spawnPointPosition[1], dataManager.spawnPointPosition[2]);
                    playerManager.currentSpawnPoint.transform.rotation = Quaternion.Euler(dataManager.spawnPointRotation[0], dataManager.spawnPointRotation[1], dataManager.spawnPointRotation[2]);
                }
            }

            UpdateHealthBar(SetMaxHealthFromHealthLevel());
            UpdateStaminaBar(SetMaxStaminaFromStaminaLevel());

            youDiedLogo.SetActive(false);
        }

        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10 + bonusHealth * 10 + Strength * 2.5f;

            return maxHealth;
        }

        public void UpdateHealthBar(float newHealth)
        {
            maxHealth = newHealth;
            currentHealth = maxHealth;

            healthBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180f * (maxHealth / 100f));
            healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(120f + (180f * (maxHealth / 100f) - 180f) / 2f, -45f);
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10 + bonusStamina * 10 + Agility * 2.5f;

            return maxStamina;
        }

        public void UpdateStaminaBar(float newStamina)
        {
            maxStamina = newStamina;
            currentStamina = maxStamina;

            staminaBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180f * (maxStamina / 100f));
            staminaBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(120f + (180f * (maxStamina / 100f) - 180f) / 2f, -80f);
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void UpdatePlayerStats()
        {
            UpdateHealthBar(SetMaxHealthFromHealthLevel());
            UpdateStaminaBar(SetMaxStaminaFromStaminaLevel());
        }

        public void TakeDamage(float damage)
        {
            if (isPlayerAlive)
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
            DropSouls();
            
            yield return new WaitForSeconds(5f);

            youDiedLogo.SetActive(false);
            playerManager.quickMoveScreen.SetActive(true);
            animatorHandler.PlayTargetAnimation("Empty", false);
            UpdateHealthBar(maxHealth);
            UpdateStaminaBar(maxStamina);
            transform.position = playerManager.currentSpawnPoint.transform.position;
            transform.rotation = playerManager.currentSpawnPoint.transform.rotation;
            // Respawn enemis and refresh boss health if alive
            RespawnEnemiesOnDead();

            yield return new WaitForSeconds(3f);

            isPlayerAlive = true;
            playerManager.quickMoveScreen.SetActive(false);
        }

        private void RespawnEnemiesOnDead()
        {
            GameObject[] enemiesSpawners = GameObject.FindGameObjectsWithTag("Spawner");
            GameObject closestSpawner = null;
            float closetDist = Mathf.Infinity;
            Vector3 playerPosition = transform.position;
            Vector3 distToTarget;
            float distSqrt;

            foreach (var eS in enemiesSpawners)
            {
                distToTarget = eS.transform.position - playerPosition;
                distSqrt = distToTarget.sqrMagnitude;

                if(distSqrt < closetDist)
                {
                    closetDist = distSqrt;
                    closestSpawner = eS;
                }
            }

            closestSpawner.GetComponent<EnemySpawner>().SpawnEnemies();
        }

        private void DropSouls()
        {
            if (soulsAmount > 0)
            {
                ConsumableItem deathDrop = new ConsumableItem();
                deathDrop.soulAmount = soulsAmount;
                deathDrop.itemName = "Souls";
                deathDrop.itemIcon = deathDropIcon;
                soulsAmount = 0;
                uiManager.UpdateSouls();
                soulDeathDrop.consumableItems = new ConsumableItem[1] { deathDrop };
                soulDeathDrop.interactableText = "Recover souls";
                Instantiate(soulDeathDrop, transform.position, Quaternion.identity);
            }
        }
    }

}