using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class BonfireManager : MonoBehaviour
    {
        [Header("Activation properties")]
        public float bonfireLitScreenTime = 2.0f;
        public bool isActivated = false;
        public bool showRestPopUp = false;
        public Light bonfireLight;
        public ParticleSystem bonfireParticleSystem;
        public GameObject bonfireLitScreen;

        [Header("UI objects")]
        public GameObject playerUI;
        public GameObject uiWindow;
        public GameObject restUI;

        [Header("Player Scripts")]
        public PlayerStats playerStats;
        public PlayerManager playerManager;

        private void Awake()
        {
            bonfireLight.enabled = false;
            bonfireParticleSystem.Stop();
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
            uiWindow.SetActive(false);
            playerUI.SetActive(true);
        }

        public void RespawnEnemis()
        {

        }
    }
}