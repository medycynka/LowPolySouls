using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SP
{
    public class AreaManager : MonoBehaviour
    {
        [Header("Area Manager", order = 0)]
        [Header("Area Name", order = 1)]
        public string areaName = "";

        [Header("Location Screen Properties", order = 1)]
        public GameObject locationScreen;
        public TextMeshProUGUI locationScreenText;

        [Header("Bonfires in Area", order = 1)]
        public BonfireManager[] bonfiresInArea;

        [Header("Bonfires in Area", order = 1)]
        public AnimationSoundManager playerSoundManager;

        [Header("Area Sounds", order = 1)]
        public AudioClip areaBgMusic;
        public AudioClip[] footSteps;
        private AudioClip[] footStepsOnExit;

        [Header("Bools", order = 1)]
        public bool isInside = false;
        public bool isPlayerDead = false;

        bool insideReset = true;

        [Header("Player Stats", order = 1)]
        public PlayerStats playerStats;

        EnemySpawner enemySpawner;

        private void Awake()
        {
            enemySpawner = GetComponentInChildren<EnemySpawner>();

            foreach(var bonfire_ in bonfiresInArea)
            {
                bonfire_.enemySpawner = enemySpawner;
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
            isInside = false;
            insideReset = true;
            playerSoundManager.ChangeBackGroundMusic(null);
            playerSoundManager.movingClips = footStepsOnExit;
        }

        private IEnumerator ShowAreaName()
        {
            locationScreenText.text = areaName;
            locationScreen.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            locationScreenText.text = "";
            locationScreen.SetActive(false);
        }
    }
}