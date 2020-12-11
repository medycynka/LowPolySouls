using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{

    public class FireplaceActivator : Interactable
    {
        public Light light;
        public ParticleSystem particleSystem;

        private void Awake()
        {
            light.enabled = false;
            particleSystem.Stop();
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            ActivateFireplace(playerManager);
        }

        private void ActivateFireplace(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            particleSystem.Play();
            light.enabled = true;

            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = "Bonefire lit";
            playerManager.itemInteractableGameObject.SetActive(true);
        }
    }

}