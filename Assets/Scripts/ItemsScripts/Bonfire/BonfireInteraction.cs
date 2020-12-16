using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SP
{
    [RequireComponent(typeof(PlayerManager))]
    public class BonfireInteraction : Interactable
    {
        BonfireManager bonfireManager;
        PlayerStats playerStats;

        private void Awake()
        {
            bonfireManager = GetComponent<BonfireManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            RestAtBonfire(playerManager);
        }

        private void RestAtBonfire(PlayerManager playerManager)
        {
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
            playerStats = playerManager.GetComponent<PlayerStats>();

            playerLocomotion.rigidbody.velocity = Vector3.zero; //Stops the player from moving whilst picking up item
            animatorHandler.PlayTargetAnimation("Sit Down", true); //Plays the animation of looting the item
            bonfireManager.playerManager.isRestingAtBonfire = true;

            playerStats.RefillHealth();
            playerStats.RefillStamina();
            bonfireManager.ActivateRestUI();
            playerManager.currentSpawnPoint = bonfireManager.spawnPoint;
            //bonfireManager.RespawnEnemis();
        }

        public void GetUp()
        {
            if (animatorHandler == null)
            {
                animatorHandler = bonfireManager.playerManager.GetComponentInChildren<AnimatorHandler>();
            }

            bonfireManager.uiManager.UpdateSouls();
            bonfireManager.CloseRestUI();
            animatorHandler.PlayTargetAnimation("Stand Up", true);
            bonfireManager.playerManager.isRestingAtBonfire = false;
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

            yield return new WaitForSeconds(bonfireManager.quickMoveScreenTime);

            bonfireManager.CloseQuickMoveScreen();
            animatorHandler.PlayTargetAnimation("Stand Up", true);
            bonfireManager.locationScreen.SetActive(true);
            bonfireManager.locationScreen.GetComponentInChildren<TextMeshProUGUI>().text = bonfireManager.locationName;

            yield return new WaitForSeconds(1.5f);

            bonfireManager.locationScreen.SetActive(false);
            bonfireManager.playerManager.isRestingAtBonfire = false;
            bonfireManager.playerManager.currentSpawnPoint = bonfireManager.spawnPoint;
        }
    }
}