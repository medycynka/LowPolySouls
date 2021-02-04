using System.Collections;
using UnityEngine;
using SzymonPeszek.SaveScripts;
using SzymonPeszek.GameUI.StatBars;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.GameUI;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Environment.Areas;
using SzymonPeszek.Misc;
using SzymonPeszek.Enums;
using SzymonPeszek.EnemyScripts;


namespace SzymonPeszek.PlayerScripts
{
    public class PlayerStats : CharacterStats
    {
        private PlayerManager _playerManager;
        private WeaponSlotManager _weaponSlotManager;
        private PlayerAnimatorHandler _playerAnimatorHandler;

        [Header("Player Properties", order = 1)]

        [Header("UI Components", order = 2)]
        public UIManager uiManager;
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusBar focusBar;
        public GameObject youDiedLogo;

        [Header("Death Drop", order = 2)]
        public Sprite deathDropIcon;
        public ConsumablePickUp soulDeathDrop;
        public Vector3 jumpDeathDropPosition;

        [Header("Unique Player Stats", order = 2)]
        public string playerName = SettingsHolder.playerName;
        public int playerLevel = 12;
        public float soulsAmount = 0;
        public float currentArmorValue;
        public float bonusBuffAttack = 1.0f;
        public float bonusBuffDefence = 1.0f;
        public float bonusBuffMagic = 1.0f;
        public float bonusBuffEndurance = 1.0f;

        [Header("Health & Stamina refill values", order = 2)]
        public float healthRefillAmount = 20f;
        public float healthBgRefillAmount = 20f;
        public float staminaRefillAmount = 20f;
        public float focusRefillAmount = 20f;

        [Header("Bools", order = 2)]
        public bool isPlayerAlive = true;
        public bool isJumpDeath;

        private EnemySpawner[] _enemiesSpawners;
        private RectTransform _hpBarTransform;
        private RectTransform _staminaBarTransform;
        private RectTransform _focusBarTransform;

        private void Awake()
        {
            _playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            uiManager = FindObjectOfType<UIManager>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusBar = FindObjectOfType<FocusBar>();
            characterTransform = GetComponent<Transform>();
        }

        private void Start()
        {
            _playerManager = GetComponent<PlayerManager>();
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            _enemiesSpawners = GameObject.FindObjectsOfType<EnemySpawner>();
            youDiedLogo.SetActive(false);

            DataManager dataManager = SettingsHolder.dataManager;

            if (dataManager != null)
            {
                if (!dataManager.isFirstStart)
                {
                    currentHealth = dataManager.currentHealth;
                    currentStamina = dataManager.currentStamina;
                    baseArmor = dataManager.baseArmor;
                    strength = dataManager.strength;
                    agility = dataManager.agility;
                    defence = dataManager.defence;
                    bonusHealth = dataManager.bonusHealth;
                    bonusStamina = dataManager.bonusStamina;
                    playerLevel = dataManager.playerLevel;
                    soulsAmount = dataManager.soulsAmount;

                    gameObject.transform.position = new Vector3(dataManager.spawnPointPosition[0], dataManager.spawnPointPosition[1], dataManager.spawnPointPosition[2]);
                    gameObject.transform.rotation = Quaternion.Euler(dataManager.spawnPointRotation[0], dataManager.spawnPointRotation[1], dataManager.spawnPointRotation[2]);
                    _playerManager.currentSpawnPoint.transform.position = new Vector3(dataManager.spawnPointPosition[0], dataManager.spawnPointPosition[1], dataManager.spawnPointPosition[2]);
                    _playerManager.currentSpawnPoint.transform.rotation = Quaternion.Euler(dataManager.spawnPointRotation[0], dataManager.spawnPointRotation[1], dataManager.spawnPointRotation[2]);
                }
                else
                {
                    SettingsHolder.firstStart = false;
                }
            }

            _hpBarTransform = healthBar.GetComponent<RectTransform>();
            _staminaBarTransform = staminaBar.GetComponent<RectTransform>();
            _focusBarTransform = focusBar.GetComponent<RectTransform>();
            
            UpdateHealthBar(SetMaxHealthFromHealthLevel());
            UpdateStaminaBar(SetMaxStaminaFromStaminaLevel());
            UpdateFocusBar(SetMaxFocusFromFocusLevel());
        }

        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10 + bonusHealth * 10 + strength * 2.5f;

            return maxHealth;
        }

        private void UpdateHealthBar(float newHealth)
        {
            maxHealth = newHealth;
            currentHealth = maxHealth;

            float currentPixelWidth = 180f * (maxHealth / 100f);
            float remappedPixelWidth = currentPixelWidth.Remap(100.0f, 1337.5f, 0.0f, 1.0f);
            float widthToSet = Mathf.Lerp(180.0f, Screen.width - Mathf.Lerp(60, 120, remappedPixelWidth), remappedPixelWidth);

            _hpBarTransform.anchoredPosition = new Vector2(120f + (widthToSet - 180.0f) / 2f, -45f);
            _hpBarTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthToSet);
            
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10 + bonusStamina * 10 + agility * 2.5f;

            return maxStamina;
        }

        private void UpdateStaminaBar(float newStamina)
        {
            maxStamina = newStamina;
            currentStamina = maxStamina;

            float currentPixelWidth = 180f * (maxStamina / 100f);
            float remappedPixelWidth = currentPixelWidth.Remap(100.0f, 1337.5f, 0.0f, 1.0f);
            float widthToSet = Mathf.Lerp(180.0f, Screen.width - Mathf.Lerp(60, 120, remappedPixelWidth), remappedPixelWidth);

            _staminaBarTransform.anchoredPosition = new Vector2(120f + (widthToSet - 180f) / 2f, -80f);
            _staminaBarTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthToSet);
            
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);
        }
        
        private float SetMaxFocusFromFocusLevel()
        {
            maxFocus = focusLevel * 10;

            return maxFocus;
        }

        private void UpdateFocusBar(float newFocus)
        {
            maxFocus = newFocus;
            currentFocus = maxFocus;

            float currentPixelWidth = 180f * (maxFocus / 100f);
            float remappedPixelWidth = currentPixelWidth.Remap(100.0f, 1337.5f, 0.0f, 1.0f);
            float widthToSet = Mathf.Lerp(180.0f, Screen.width - Mathf.Lerp(60, 120, remappedPixelWidth), remappedPixelWidth);

            _focusBarTransform.anchoredPosition = new Vector2(120f + (widthToSet - 180f) / 2f, -115f);
            _focusBarTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthToSet);
            
            focusBar.SetMaxFocus(maxFocus);
            focusBar.SetCurrentFocus(currentFocus);
        }

        public void UpdatePlayerStats()
        {
            UpdateHealthBar(SetMaxHealthFromHealthLevel());
            UpdateStaminaBar(SetMaxStaminaFromStaminaLevel());
            //UpdateFocusBar(SetMaxFocusFromFocusLevel());
        }

        public void TakeDamage(float damage, bool isBackStabbed = false, bool isRiposted = false)
        {
            if (isPlayerAlive && !_playerManager.isInvulnerable)
            {
                _playerManager.shouldRefillHealth = false;
                currentHealth -= damage;
                healthBar.SetCurrentHealth(currentHealth);

                _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.Damage01Name], true);

                if (currentHealth <= 0)
                {
                    HandleDeathAndRespawn(isBackStabbed);
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

        public void HealPlayer(float healAmount)
        {
            currentHealth += healAmount * bonusBuffMagic;

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.healthBarSlider.value += healAmount;
            healthBar.backgroundSlider.value += healAmount;
        }

        public void BuffPlayer(StatsBuffType buffType, BuffRang buffRang, float value)
        {
            switch (buffType)
            {
                case StatsBuffType.Attack:
                    StopCoroutine(BuffAttack(buffRang, value));
                    StartCoroutine(BuffAttack(buffRang, value));
                    break;
                case StatsBuffType.Defence:
                    StopCoroutine(BuffDefence(buffRang, value));
                    StartCoroutine(BuffDefence(buffRang, value));
                    break;
                case StatsBuffType.MagicAttack:
                    StopCoroutine(BuffMagic(buffRang, value));
                    StartCoroutine(BuffMagic(buffRang, value));
                    break;
                case StatsBuffType.Endurance:
                    StopCoroutine(BuffEndurance(buffRang, value));
                    StartCoroutine(BuffEndurance(buffRang, value));
                    break;
            }
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
        
        public void TakeFocusDamage(float drain)
        {
            currentFocus = currentFocus - drain;

            if(currentFocus < 0)
            {
                currentFocus = 0;
            }

            focusBar.SetCurrentFocus(currentFocus);
        }

        public void RefillFocus()
        {
            currentFocus += focusRefillAmount * Time.deltaTime;

            if (currentFocus > maxFocus)
            {
                currentFocus = maxFocus;
            }

            focusBar.focusBarSlider.value += focusRefillAmount * Time.deltaTime;
        }
        
        public void DealDamage(EnemyStats enemyStats, float weaponDamage)
        {
            enemyStats.TakeDamage((weaponDamage * _weaponSlotManager.attackingWeapon.lightAttackDamageMult + strength * 0.5f) * bonusBuffAttack, false);
        }

        public int CalculateSoulsCost(int level)
        {
            return (int)(0.02f * level * level * level + 3.06f * level * level + 105.6f * level - 895f);
        }

        private void HandleDeathAndRespawn(bool isBackStabbed)
        {
            currentHealth = 0;
            _playerAnimatorHandler.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsDeadName], true);
            
            if (isJumpDeath)
            {
                _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.LayDownName], true);
            }
            else if(isBackStabbed)
            {
                _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.BackStabName], true);
            }
            else
            {
                _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.Death01Name], true);
            }

            isPlayerAlive = false;

            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            youDiedLogo.SetActive(true);
            DropSouls(isJumpDeath ? jumpDeathDropPosition : transform.position);

            yield return CoroutineYielder.playerRespawnWaiter;

            youDiedLogo.SetActive(false);
            _playerManager.quickMoveScreen.SetActive(true);
            _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.EmptyName], false);
            UpdateHealthBar(maxHealth);
            UpdateStaminaBar(maxStamina);
            characterTransform.position = _playerManager.currentSpawnPoint.transform.position;
            characterTransform.rotation = _playerManager.currentSpawnPoint.transform.rotation;
            // Respawn enemies and refresh boss health if alive
            RespawnEnemiesOnDead();

            yield return CoroutineYielder.playerRespawnWaiter;
            
            isPlayerAlive = true;
            _playerAnimatorHandler.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsDeadName], false);
            _playerManager.quickMoveScreen.SetActive(false);

            if (isJumpDeath)
            {
                isJumpDeath = false;
            }
        }
        
        private void RespawnEnemiesOnDead()
        {
            foreach (var eS in _enemiesSpawners)
            {
                eS.SpawnEnemies();
            }
        }

        private void DropSouls(Vector3 dropPosition)
        {
            if (soulsAmount > 0)
            {
                ConsumableItem deathDrop = ScriptableObject.CreateInstance<ConsumableItem>();
                deathDrop.soulAmount = soulsAmount;
                deathDrop.itemName = "Souls recovered";
                deathDrop.itemIcon = deathDropIcon;
                deathDrop.consumableType = ConsumableType.SoulItem;
                deathDrop.isDeathDrop = true;
                soulsAmount = 0;
                uiManager.UpdateSouls();
                soulDeathDrop.consumableItems = new []{ deathDrop };
                soulDeathDrop.interactableText = "Recover souls";
                Instantiate(soulDeathDrop, dropPosition, Quaternion.identity);
            }
        }

        private IEnumerator BuffAttack(BuffRang buffRang, float value)
        {
            bonusBuffAttack = value;

            switch (buffRang)
            {
                case BuffRang.Lesser:
                    yield return CoroutineYielder.lesserBuffWaiter;
                    break;
                case BuffRang.Medium:
                    yield return CoroutineYielder.mediumBuffWaiter;
                    break;
                case BuffRang.Grand:
                    yield return CoroutineYielder.grandBuffWaiter;
                    break;
                default:
                    yield return null;
                    break;
            }

            bonusBuffAttack = 1.0f;
        }
        
        private IEnumerator BuffDefence(BuffRang buffRang, float value)
        {
            bonusBuffDefence = value;

            switch (buffRang)
            {
                case BuffRang.Lesser:
                    yield return CoroutineYielder.lesserBuffWaiter;
                    break;
                case BuffRang.Medium:
                    yield return CoroutineYielder.mediumBuffWaiter;
                    break;
                case BuffRang.Grand:
                    yield return CoroutineYielder.grandBuffWaiter;
                    break;
                default:
                    yield return null;
                    break;
            }

            bonusBuffDefence = 1.0f;
        }
        
        private IEnumerator BuffMagic(BuffRang buffRang, float value)
        {
            bonusBuffMagic = value;

            switch (buffRang)
            {
                case BuffRang.Lesser:
                    yield return CoroutineYielder.lesserBuffWaiter;
                    break;
                case BuffRang.Medium:
                    yield return CoroutineYielder.mediumBuffWaiter;
                    break;
                case BuffRang.Grand:
                    yield return CoroutineYielder.grandBuffWaiter;
                    break;
                default:
                    yield return null;
                    break;
            }

            bonusBuffAttack = 1.0f;
        }
        
        private IEnumerator BuffEndurance(BuffRang buffRang, float value)
        {
            bonusBuffEndurance = value;

            switch (buffRang)
            {
                case BuffRang.Lesser:
                    yield return CoroutineYielder.lesserBuffWaiter;
                    break;
                case BuffRang.Medium:
                    yield return CoroutineYielder.mediumBuffWaiter;
                    break;
                case BuffRang.Grand:
                    yield return CoroutineYielder.grandBuffWaiter;
                    break;
                default:
                    yield return null;
                    break;
            }

            bonusBuffEndurance = 1.0f;
        }
    }
}