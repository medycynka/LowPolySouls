using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class FogWallManager : Interactable
    {
        [Header("Fog Wall Manager", order = 1)]
        [Header("Fog Wall Components", order = 2)]
        public BoxCollider boxCollider;
        public ParticleSystem wallParticles;

        [Header("Bools", order = 2)]
        public bool canInteract = true;
        public bool shouldDestroy = false;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        public override void PickUpItem(PlayerManager playerManager)
        {
            boxCollider.enabled = false;
            wallParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            base.PickUpItem(playerManager);

            if (shouldDestroy)
            {
                StartCoroutine(DestroyFog(playerManager));
            }
            else
            {
                StartCoroutine(RemoveFog(playerManager));
            }
        }

        private IEnumerator DestroyFog(PlayerManager playerManager)
        {
            playerManager.isRemovigFog = true;

            yield return new WaitForSeconds(2f);

            playerManager.isRemovigFog = false;

            Destroy(gameObject);
        }

        private IEnumerator RemoveFog(PlayerManager playerManager)
        {
            canInteract = false;
            playerManager.isRemovigFog = true;

            yield return new WaitForSeconds(2f);

            playerManager.isRemovigFog = false;

            yield return new WaitForSeconds(5f);

            boxCollider.enabled = true;
            wallParticles.Play(true);
            canInteract = true;
        }
    }
}