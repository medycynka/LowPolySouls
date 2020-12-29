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

            StartCoroutine(RemoveFog(playerManager));
        }

        private IEnumerator RemoveFog(PlayerManager playerManager)
        {
            playerManager.isRemovigFog = true;

            yield return new WaitForSeconds(2f);

            playerManager.isRemovigFog = false;
            Destroy(this.gameObject);
        }
    }
}