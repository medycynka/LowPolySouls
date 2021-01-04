using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SP
{
    public class BossAreaManager : LocationManager
    {
        [Header("Area Manager", order = 0)]
        AudioClip[] footStepsOnExit;
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

        private void Start()
        {
            if (isBossAlive)
            {
                bonfiresInArea[0].gameObject.SetActive(false);
                bossStats = bossPrefab.GetComponent<EnemyStats>();
                bossHpBar.GetComponentInChildren<TextMeshProUGUI>().text = bossName;
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
            }
        }

        public void SetExitFootSteps(AudioClip[] footSteps)
        {
            footStepsOnExit = footSteps;
        }

        private void OnTriggerEnter(Collider other)
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
                    foreach (var fogWall in fogWalls)
                    {
                        fogWall.canInteract = false;
                    }
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

        private void OnTriggerStay(Collider other)
        {
            if (isInside)
            {
                insideReset = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            playerStats = null;
            isInside = false;
            insideReset = true;
            playerSoundManager.ChangeBackGroundMusic(null);
            playerSoundManager.movingClips = footStepsOnExit;
            bossHpBar.SetActive(false);
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
                }
            }
        }

        private IEnumerator ShowAreaName()
        {
            locationScreenText.text = areaName;
            locationScreen.SetActive(true);

            if (isBossAlive)
            {
                bossHpBar.SetActive(true);
            }

            yield return new WaitForSeconds(1.5f);

            locationScreenText.text = "";
            locationScreen.SetActive(false);
        }

        private IEnumerator HealBoss()
        {
            bossHpBar.SetActive(false);

            yield return new WaitForSeconds(1f);

            bossStats.InitializeHealth();

            yield return new WaitForSeconds(5f);

            bossPrefab.transform.position = startPosition;
        }
    }
}