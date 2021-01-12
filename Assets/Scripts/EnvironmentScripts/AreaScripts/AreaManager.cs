using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SP
{
    public class AreaManager : LocationManager
    {
        [Header("Area Manager", order = 0)]
        EnemySpawner enemySpawner;
        bool insideReset = true;
        AudioClip[] footStepsOnExit;

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
                if (playerStats == null)
                {
                    playerStats = other.GetComponent<PlayerStats>();
                }

                if (playerSoundManager == null)
                {
                    playerSoundManager = other.GetComponent<AnimationSoundManager>();
                }

                if (playerSoundManager.fadingMusic)
                {
                    // Reset fading
                    playerSoundManager.fadingMusic = false;
                }
                
                playerSoundManager.fadingMusic = true;
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
            
            if (playerSoundManager.fadingMusic)
            {
                // Reset fading
                playerSoundManager.fadingMusic = false;
            }
            
            playerSoundManager.fadingMusic = true;
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