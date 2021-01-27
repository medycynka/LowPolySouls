using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SP
{
    public class BossAreaManager : LocationManager
    {
        [Header("Area Manager", order = 0)]
        AudioClip[] footStepsOnExit;
        public AudioClip bgMusicBossDefeat;
        bool insideReset = true;

        [Header("Fog Walls")]
        public FogWallManager[] fogWalls;

        [Header("Boss Prefab")]
        public string bossName = "";
        public GameObject bossPrefab;
        public Vector3 startPosition;
        public GameObject bossHpBar;
        public bool isBossAlive = true;

        EnemyStats bossStats;
        Slider bossHpSlider;
        TextMeshProUGUI bossNameText;

        private void Start()
        {
            if (isBossAlive)
            {
                bonfiresInArea[0].gameObject.SetActive(false);
                bossStats = bossPrefab.GetComponent<EnemyStats>();
                bossStats.bossAreaManager = this;
                bossHpBar.SetActive(false);
                bossHpSlider = bossHpBar.GetComponentInChildren<Slider>();
                bossNameText = bossHpBar.GetComponentInChildren<TextMeshProUGUI>();
            }
            else
            {
                if (bossPrefab != null)
                {
                    Destroy(bossPrefab.gameObject);
                }

                foreach (var fogWall in fogWalls)
                {
                    Destroy(fogWall.gameObject);
                }

                bonfiresInArea[0].gameObject.SetActive(true);
                bonfiresInArea[0].bonfireParticleSystem.Play();
                bonfiresInArea[0].bonfireLight.enabled = true;
            }
        }

        public void SetExitFootSteps(AudioClip[] footSteps)
        {
            footStepsOnExit = footSteps;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                isInside = true;

                if (insideReset)
                {
                    if (playerStats == null)
                    {
                        playerStats = other.GetComponent<PlayerStats>();
                    }

                    if (isBossAlive)
                    {
                        bossHpSlider.minValue = 0;
                        bossHpSlider.maxValue = bossStats.maxHealth;
                        bossHpSlider.value = bossStats.maxHealth;
                    }

                    if (playerSoundManager == null)
                    {
                        playerSoundManager = other.GetComponent<AnimationSoundManager>();
                    }

                    playerSoundManager.ChangeBackGroundMusic(areaBgMusic);
                    playerSoundManager.ChangeFootstepsSound(footSteps, this);

                    StartCoroutine(ShowAreaName());
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                if (isInside && insideReset)
                {
                    insideReset = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerStats = null;
                isInside = false;
                insideReset = true;
                playerSoundManager.ChangeBackGroundMusic(null);
                playerSoundManager.movingClips = footStepsOnExit;
                bossHpBar.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (playerStats != null)
            {
                if (playerStats.currentHealth <= 0)
                {
                    StartCoroutine(HealBoss());
                }
            }

            if (isBossAlive)
            {
                if (bossStats.currentHealth <= 0)
                {
                    foreach (var fogWall in fogWalls)
                    {
                        Destroy(fogWall.gameObject);
                    }

                    isBossAlive = false;
                    bonfiresInArea[0].gameObject.SetActive(true);
                    if (playerSoundManager != null)
                    {
                        playerSoundManager.ChangeBackGroundMusic(bgMusicBossDefeat);
                    }

                    areaBgMusic = bgMusicBossDefeat;
                }
            }
        }

        private IEnumerator ShowAreaName()
        {
            locationScreenText.text = areaName;
            locationScreen.SetActive(true);

            if (isBossAlive)
            {
                bossNameText.text = bossName;
                bossHpBar.SetActive(true);
            }

            yield return CoroutineYielder.areaNameWaiter;

            locationScreenText.text = "";
            locationScreen.SetActive(false);
        }

        private IEnumerator HealBoss()
        {
            bossHpBar.SetActive(false);
            fogWalls[0].canInteract = true;

            yield return CoroutineYielder.bossHealWaiter;

            bossStats.InitializeHealth();

            yield return CoroutineYielder.bossPositionResetWaiter;

            bossPrefab.transform.position = startPosition;
        }
    }
}