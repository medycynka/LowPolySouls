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
        public Light bonfireLight;
        public ParticleSystem bonfireParticleSystem;
        public GameObject bonfireLitScreen;

        [Header("UI objects")]
        public GameObject playerUI;
        public GameObject restUI;

        [Header("Player Stats")]
        public PlayerStats playerStats;

        private void Awake()
        {
            bonfireLight.enabled = false;
            bonfireParticleSystem.Stop();
        }

        public void ActivateRestUI()
        {
            playerUI.SetActive(false);
            restUI.SetActive(true);
        }

        public void DeactivateRestUI()
        {
            restUI.SetActive(false);
            playerUI.SetActive(true);
        }
    }
}