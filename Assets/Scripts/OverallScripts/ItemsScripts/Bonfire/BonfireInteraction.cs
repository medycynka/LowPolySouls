using System.Collections;
using UnityEngine;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.GameUI.WindowsManagers;
using SzymonPeszek.Misc;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.Items.Bonfire
{
    public class BonfireInteraction : Interactable
    {
        private BonfireManager _bonfireManager;
        private PlayerStats _playerStats;
        private TextMeshProUGUI _locationNameScree;

        private void Awake()
        {
            _bonfireManager = GetComponent<BonfireManager>();
            _locationNameScree = _bonfireManager.locationScreen.GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            RestAtBonfire(playerManager);
        }

        private void RestAtBonfire(PlayerManager playerManager)
        {
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
            _playerStats = playerManager.GetComponent<PlayerStats>();
            _bonfireManager.restUI.GetComponent<RestManager>().bonfireInteraction = this;

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.SitName], true);
            _bonfireManager.playerManager.isRestingAtBonfire = true;

            _playerStats.RefillHealth();
            _playerStats.RefillStamina();
            _bonfireManager.ActivateRestUI();
            playerManager.currentSpawnPoint = _bonfireManager.spawnPoint;
            _bonfireManager.RespawnEnemies();

            SaveManager.SaveGame(playerManager, _playerStats, playerManager.GetComponent<PlayerInventory>());
        }

        public void GetUp()
        {
            if (animatorHandler == null)
            {
                animatorHandler = _bonfireManager.playerManager.GetComponentInChildren<AnimatorHandler>();
            }

            _bonfireManager.uiManager.UpdateSouls();
            _bonfireManager.CloseRestUI();
            animatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.StandUpName], true);
            _bonfireManager.playerManager.isRestingAtBonfire = false;

            if(_playerStats == null)
            {
                _playerStats = _bonfireManager.playerManager.GetComponent<PlayerStats>();
            }
        }

        public void QuickMove()
        {
            if(animatorHandler == null)
            {
                animatorHandler = _bonfireManager.playerManager.GetComponentInChildren<AnimatorHandler>();
            }

            StartCoroutine(TeleportToNextBonfire());
        }

        private IEnumerator TeleportToNextBonfire()
        {
            _bonfireManager.ActivateQuickMoveScreen();
            _bonfireManager.uiManager.UpdateSouls();
            _playerStats.characterTransform.position = _bonfireManager.spawnPoint.transform.position;
            _playerStats.characterTransform.rotation = _bonfireManager.spawnPoint.transform.rotation;

            yield return CoroutineYielder.bonfireTeleportFirstWaiter;

            _bonfireManager.CloseQuickMoveScreen();
            animatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.StandUpName], true);
            _bonfireManager.locationScreen.SetActive(true);
            _locationNameScree.text = _bonfireManager.locationName;

            yield return CoroutineYielder.bonfireTeleportSecondWaiter;

            _bonfireManager.locationScreen.SetActive(false);
            _bonfireManager.playerManager.isRestingAtBonfire = false;
            _bonfireManager.playerManager.currentSpawnPoint = _bonfireManager.spawnPoint;
        }
    }
}