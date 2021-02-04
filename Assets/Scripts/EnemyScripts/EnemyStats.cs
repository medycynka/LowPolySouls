using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Environment.Areas;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts
{
    public class EnemyStats : CharacterStats
    {
        private EnemyManager _enemyManager;

        [Header("Enemy Properties", order = 1)]
        [Header("Animator", order = 2)]
        public EnemyAnimationManager animator;

        [Header("Health Bar", order = 2)]
        public GameObject healthBar;
        public Image healthBarFill;
        public TextMeshProUGUI damageValue;

        [Header("Souls & souls target", order = 2)]
        public float soulsGiveAmount;
        public PlayerStats playerStats;
        public GameObject enemyObject;

        [Header("Is it Boss", order = 2)]
        public bool isBoss;
        public BossAreaManager bossAreaManager;
        public Slider bossHpSlider;

        private Transform _mainCameraTransform;

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
        }

        void Start()
        {
            InitializeHealth();
        }

        private void LateUpdate()
        {
            if (!isBoss)
            {
                healthBar.transform.LookAt(_mainCameraTransform);
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
            }
            else
            {
                maxHealth = SetMaxHealthFromHealthLevel();
                currentHealth = maxHealth;

                if (healthBarFill == null)
                {
                    healthBarFill = healthBar.GetComponentInChildren<Image>();
                }
                
                healthBarFill.fillAmount = 1f;
                healthBar.SetActive(false);
            }
        }

        public void TakeDamage(float damage, bool isBackStabbed, bool isRiposted = false)
        {
            if (_enemyManager.isAlive)
            {
                if (isBoss)
                {
                    currentHealth -= damage;
                    bossHpSlider.value = currentHealth;

                    if (currentHealth > 0)
                    {
                        animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.Damage01Name], true);
                    }
                    else
                    {
                        _enemyManager.deadFromBackStab = isBackStabbed;
                        bossAreaManager.bossHpBar.SetActive(false);
                    }
                }
                else
                {
                    StartCoroutine(UpdateEnemyHealthBar(damage, isBackStabbed, isRiposted));
                }
            }
        }

        public void DealDamage(PlayerStats playerStat, float weaponDamage)
        {
            playerStat.TakeDamage(weaponDamage + strength);
        }

        private IEnumerator UpdateEnemyHealthBar(float damage, bool isBackStabbed, bool isRiposted)
        {
            _enemyManager.deadFromBackStab = (isBackStabbed && currentHealth - damage <= 0.0f);
            healthBar.SetActive(true);
            currentHealth -= damage;
            healthBarFill.fillAmount = currentHealth / maxHealth;
            damageValue.text = damage.ToString();

            if (currentHealth > 0)
            {
                animator.PlayTargetAnimation(isBackStabbed ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.BackStabbedName] : StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.Damage01Name], true);
            }

            yield return CoroutineYielder.enemyHpUpdateWaiter;

            healthBar.SetActive(false);
        }
    }

}
