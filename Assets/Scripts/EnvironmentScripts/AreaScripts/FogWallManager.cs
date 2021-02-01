using System.Collections;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Environment.Areas
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
            playerManager.isRemovingFog = true;
            wallParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

            yield return CoroutineYielder.fogWallDestroyWaiter;

            playerManager.isRemovingFog = false;

            Destroy(gameObject);
        }

        private IEnumerator RemoveFog(PlayerManager playerManager)
        {
            canInteract = false;
            playerManager.isRemovingFog = true;
            
            yield return CoroutineYielder.fogWallRemoveFirstWaiter;
            
            animatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.FogRemoveName], true);

            yield return CoroutineYielder.fogWallRemoveSecondWaiter;

            playerManager.isRemovingFog = false;
            boxCollider.enabled = true;
        }
    }
}