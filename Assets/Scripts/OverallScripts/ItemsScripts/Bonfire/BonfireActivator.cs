using System.Collections;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Items.Bonfire
{

    public class BonfireActivator : Interactable
    {
        private BonfireManager _bonfireManager;

        private void Awake()
        {
            _bonfireManager = GetComponent<BonfireManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            ActivateFireplace(playerManager);
        }

        private void ActivateFireplace(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            _bonfireManager.bonfireParticleSystem.Play();
            _bonfireManager.bonfireLight.enabled = true;
            _bonfireManager.isActivated = true;

            StartCoroutine(DisplayScreen());
        }

        private IEnumerator DisplayScreen()
        {
            _bonfireManager.bonfireLitScreen.SetActive(true);

            yield return CoroutineYielder.bonfireLitWaiter;

            _bonfireManager.bonfireLitScreen.SetActive(false);
            _bonfireManager.showRestPopUp = true;
        }
    }

}