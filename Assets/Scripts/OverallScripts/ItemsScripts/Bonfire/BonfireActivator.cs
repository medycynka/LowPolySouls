using System.Collections;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Items.Bonfire
{

    public class BonfireActivator : Interactable
    {
        BonfireManager bonfireManager;

        private void Awake()
        {
            bonfireManager = GetComponent<BonfireManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            ActivateFireplace(playerManager);
        }

        private void ActivateFireplace(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            bonfireManager.bonfireParticleSystem.Play();
            bonfireManager.bonfireLight.enabled = true;
            bonfireManager.isActivated = true;

            StartCoroutine(DisplayScreen());
        }

        private IEnumerator DisplayScreen()
        {
            bonfireManager.bonfireLitScreen.SetActive(true);

            yield return CoroutineYielder.bonfireLitWaiter;

            bonfireManager.bonfireLitScreen.SetActive(false);
            bonfireManager.showRestPopUp = true;
        }
    }

}