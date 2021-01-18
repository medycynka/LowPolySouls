using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SP
{
    public class BonfireInteraction : Interactable
    {
        BonfireManager bonfireManager;
        PlayerStats playerStats;
        private TextMeshProUGUI locationNameScree;

        private void Awake()
        {
            bonfireManager = GetComponent<BonfireManager>();
            locationNameScree = bonfireManager.locationScreen.GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            RestAtBonfire(playerManager);
        }

        private void RestAtBonfire(PlayerManager playerManager)
        {
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
            playerStats = playerManager.GetComponent<PlayerStats>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation(StaticAnimatorIds.SitId, true);
            bonfireManager.playerManager.isRestingAtBonfire = true;

            playerStats.RefillHealth();
            playerStats.RefillStamina();
            bonfireManager.ActivateRestUI();
            playerManager.currentSpawnPoint = bonfireManager.spawnPoint;
            bonfireManager.RespawnEnemis();

            SaveManager.SaveGame(playerManager, playerStats, playerManager.GetComponent<PlayerInventory>());
        }

        public void GetUp()
        {
            if (animatorHandler == null)
            {
                animatorHandler = bonfireManager.playerManager.GetComponentInChildren<AnimatorHandler>();
            }

            bonfireManager.uiManager.UpdateSouls();
            bonfireManager.CloseRestUI();
            animatorHandler.PlayTargetAnimation(StaticAnimatorIds.StandUpId, true);
            bonfireManager.playerManager.isRestingAtBonfire = false;

            if(playerStats == null)
            {
                playerStats = bonfireManager.playerManager.GetComponent<PlayerStats>();
            }

            SaveManager.SaveGame(playerStats.GetComponent<PlayerManager>(), playerStats, playerStats.GetComponent<PlayerInventory>());
        }

        public void QuickMove()
        {
            if(animatorHandler == null)
            {
                animatorHandler = bonfireManager.playerManager.GetComponentInChildren<AnimatorHandler>();
            }

            StartCoroutine(TeleportToNextBonfire());
        }

        private IEnumerator TeleportToNextBonfire()
        {
            bonfireManager.ActivateQuickMoveScreen();
            bonfireManager.uiManager.UpdateSouls();
            bonfireManager.playerManager.transform.position = bonfireManager.spawnPoint.transform.position;
            bonfireManager.playerManager.transform.rotation = bonfireManager.spawnPoint.transform.rotation;

            yield return CoroutineYielder.bonfireTeleportFirstWaiter;

            bonfireManager.CloseQuickMoveScreen();
            animatorHandler.PlayTargetAnimation(StaticAnimatorIds.StandUpId, true);
            bonfireManager.locationScreen.SetActive(true);
            locationNameScree.text = bonfireManager.locationName;

            yield return CoroutineYielder.bonfireTeleportSecondWaiter;

            bonfireManager.locationScreen.SetActive(false);
            bonfireManager.playerManager.isRestingAtBonfire = false;
            bonfireManager.playerManager.currentSpawnPoint = bonfireManager.spawnPoint;
        }
    }
}