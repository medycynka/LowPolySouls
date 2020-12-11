using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{
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

            playerStats.RefillHealth();
            bonfireManager.ActivateRestUI();
            //bonfireManager.RespawnEnemis();
        }

        public void GetUp()
        {
            bonfireManager.CloseRestUI();
            animatorHandler.PlayTargetAnimation("Stand Up", true);
        }
    }
}