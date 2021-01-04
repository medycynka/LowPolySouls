using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class BonfireManager : MonoBehaviour
    {
        [Header("Bonfire Manager", order = 0)]
        [Header("Activation properties", order = 1)]
        public float bonfireLitScreenTime = 2.0f;
        public bool isActivated = false;
        public bool showRestPopUp = false;
        public Light bonfireLight;
        public ParticleSystem bonfireParticleSystem;
        public GameObject bonfireLitScreen;

        [Header("UI objects", order = 1)]
        public UIManager uiManager;
        public GameObject playerUI;
        public GameObject uiWindow;
        public GameObject restUI;
        public GameObject quickMoveScreen;

        [Header("Player Scripts", order = 1)]
        public PlayerStats playerStats;
        public PlayerManager playerManager;

        [Header("Quick Move", order = 1)]
        public string locationName = "Test Field";
        public GameObject locationListScreen;
        public GameObject locationScreen;
        public int qucikMoveID = 0;
        public GameObject spawnPoint;
        public float quickMoveScreenTime = 5.0f;

        [Header("Enemy Spawner", order = 1)]
        public bool isBossBonfire = false;
        public EnemySpawner enemySpawner;

        private void Awake()
        {
            if (isActivated)
            {
                bonfireParticleSystem.Play();
                bonfireLight.enabled = true;
            }
            else
            {
                bonfireLight.enabled = false;
                bonfireParticleSystem.Stop();
            }
        }

        public void ActivateRestUI()
        {
            playerUI.SetActive(false);
            uiWindow.SetActive(true);
            restUI.SetActive(true);
        }

        public void CloseRestUI()
        {
            restUI.SetActive(false);
            locationListScreen.SetActive(false);
            uiWindow.SetActive(false);
            playerUI.SetActive(true);
        }

        public void ActivateQuickMoveScreen()
        {
            locationListScreen.SetActive(false);
            restUI.SetActive(false);
            uiWindow.SetActive(false);
            playerUI.SetActive(true);
            quickMoveScreen.SetActive(true);
        }

        public void CloseQuickMoveScreen()
        {
            quickMoveScreen.SetActive(false);
        }

        public void RespawnEnemis()
        {
            if (!isBossBonfire)
            {
                enemySpawner.SpawnEnemies();
            }
        }
    }
}