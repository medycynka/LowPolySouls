using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Environment.Areas;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;
using SzymonPeszek.Enums;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class for managing enemy's stats
    /// </summary>
    public class EnemyStats : CharacterStats
    {
        [Header("Enemy Properties", order = 1)]
        [Header("Animator", order = 2)]
        public EnemyAnimationManager animator;

        [Header("Health Bar", order = 2)]
        public GameObject healthBar;
        public Image healthBarFill;
        public TextMeshProUGUI damageValue;

        [Header("Attack properties", order = 2)]
        public float enemyAttack = 25f;

        [Header("Souls & souls target", order = 2)]
        public float soulsGiveAmount;
        public PlayerStats playerStats;
        public GameObject enemyObject;

        [Header("Is it Boss", order = 2)]
        public bool isBoss;
        public BossAreaManager bossAreaManager;
        public Slider bossHpSlider;

        private EnemyManager _enemyManager;
        private Transform _mainCameraTransform;
        private const float HpBarHideTimeoutTime = 3f;
        private float _hideHpBarTimer;
        private float _accumulateDamage;

        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
            animator = GetComponentInChildren<EnemyAnimationManager>();
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            characterTransform = GetComponent<Transform>();

            if (!(Camera.main is null))
            {
                _mainCameraTransform = Camera.main.transform;
            }
            
            InitializeHealth();
        }

        private void Update()
        {
            if (healthBar)
            {
                _hideHpBarTimer -= Time.deltaTime;
                
                if (_hideHpBarTimer <= 0)
                {
                    _hideHpBarTimer = 0;
                    _accumulateDamage = 0f;
                    healthBar.SetActive(false);
                }
                else
                {
                    if (!healthBar.activeInHierarchy)
                    {
                        healthBar.SetActive(true);
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if (!isBoss)
            {
                healthBar.transform.LookAt(_mainCameraTransform);
                healthBar.transform.Rotate(0, 180, 0);
            }
        }

        /// <summary>
        /// Calculate damage received based on damage type and current modifiers
        /// </summary>
        /// <param name="damageType">Type of damage</param>
        /// <param name="damage">Damage amount</param>
        /// <returns>Calculated Damage</returns>
        protected override int CalculateDamage(DamageType damageType, float damage)
        {
            switch (damageType)
            {
                case DamageType.Physic:
                    float armorValue = Mathf.Clamp(2.5f * defence + baseArmor, 0, 999);
                    float defMod = Mathf.Clamp01(1 - Mathf.Lerp(armorValue, 999, armorValue / 999) / 999);
                    int damageMod = (int) (damage * defMod);
                    return damageMod;
                case DamageType.AbsolutePhysic:
                    return (int) damage;
                case DamageType.Magic:
                    // float magicDefMod = Mathf.Clamp01(1 - Mathf.Lerp(defence, 999, defence / 999) / 999); // Make magic defence stat
                    float magicDefMod = 1.0f;
                    int magicMod = (int) (damage * magicDefMod);
                    return magicMod;
                case DamageType.AbsoluteMagic:
                    return (int) damage;
                case DamageType.Fall:
                    return (int) damage;
                case DamageType.Other:
                    return (int) damage;
                default:
                    return (int) damage;
            }
        }

        /// <summary>
        /// Calculate maximum health value
        /// </summary>
        /// <returns>Maximum health value</returns>
        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        /// <summary>
        /// Initialize character's health
        /// </summary>
        public void InitializeHealth()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            
            if (isBoss)
            {
                if (bossHpSlider == null)
                {
                    bossHpSlider = bossAreaManager.bossHpBar.GetComponentInChildren<Slider>();
                }
            }
            else
            {
                if (healthBarFill == null)
                {
                    healthBarFill = healthBar.GetComponentInChildren<Image>();
                }
                
                healthBarFill.fillAmount = 1f;
                healthBar.SetActive(false);
            }
        }

        /// <summary>
        /// Take damage from player
        /// </summary>
        /// <param name="damage">Damage amount</param>
        /// <param name="damageType">Type of damage</param>
        /// <param name="damageAnimation">Name of damage animation</param>
        /// <param name="isBackStabbed">Is it from back stab?</param>
        /// <param name="isRiposted">Is it from riposte?</param>
        public void TakeDamage(float damage, DamageType damageType, string damageAnimation = "Damage_01", 
            bool isBackStabbed = false, bool isRiposted = false)
        {
            if (_enemyManager.isAlive)
            {
                if (isBoss)
                {
                    currentHealth -= CalculateDamage(damageType, damage);
                    bossHpSlider.value = currentHealth;

                    if (currentHealth > 0)
                    {
                        animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[damageAnimation], true);
                    }
                    else
                    {
                        _enemyManager.deadFromBackStab = isBackStabbed;
                        bossAreaManager.bossHpBar.SetActive(false);
                    }
                }
                else
                {
                    UpdateEnemyHealth(CalculateDamage(damageType, damage), isBackStabbed, isRiposted);
                }
            }
        }

        public void GetParried()
        {
            animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.ParriedName], true);
        }

        /// <summary>
        /// Deal damage to the player
        /// </summary>
        /// <param name="playerStat">Player stats</param>
        /// <param name="damageType">Type of damage</param>
        /// <param name="damageAnimation">Name of damage animation</param>
        public void DealDamage(PlayerStats playerStat, DamageType damageType, string damageAnimation = "Damage_01")
        {
            playerStat.TakeDamage(Mathf.RoundToInt(enemyAttack), damageType, damageAnimation);
        }

        /// <summary>
        /// Update enemy's health bar
        /// </summary>
        /// <param name="damage">Damage get form player</param>
        /// <param name="isBackStabbed">Is it from back stab?</param>
        /// <param name="isRiposted">Is it from riposte?</param>
        /// <returns>Coroutine's enumerator</returns>
        private void UpdateEnemyHealth(float damage, bool isBackStabbed, bool isRiposted)
        {
            _enemyManager.deadFromBackStab = (isBackStabbed && currentHealth - damage <= 0.0f);
            _enemyManager.deadFromRiposte = (isRiposted && currentHealth - damage <= 0.0f);
            _hideHpBarTimer = HpBarHideTimeoutTime;
            currentHealth -= damage;
            healthBarFill.fillAmount = currentHealth / maxHealth;
            _accumulateDamage += damage;
            damageValue.text = _accumulateDamage.ToString();

            if (currentHealth > 0)
            {
                if (isBackStabbed)
                {
                    animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.BackStabbedName],
                        true);
                }
                else if (isRiposted)
                {
                    animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.RipostedName],
                        true);
                    _enemyManager.isGettingRiposted = false;
                }
                else
                {
                    animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.Damage01Name],
                        true);
                }
            }
        }
    }

}
