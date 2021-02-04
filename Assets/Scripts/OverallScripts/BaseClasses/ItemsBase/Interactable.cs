using UnityEngine;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.GameUI;
using SzymonPeszek.Misc;


namespace SzymonPeszek.BaseClasses
{
    public class Interactable : MonoBehaviour
    {
        [Header("Interactable Object Properties")]
        public float radius = 0.6f;
        public string interactableText = "Pick up";

        [HideInInspector] public PlayerInventory playerInventory;
        [HideInInspector] public PlayerLocomotion playerLocomotion;
        [HideInInspector] public PlayerAnimatorHandler playerAnimatorHandler;
        [HideInInspector] public UIManager uIManager;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            
        }

        protected virtual void PickUpItem(PlayerManager playerManager)
        {
            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            playerAnimatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorHandler>();
            uIManager = playerManager.GetComponent<InputHandler>().uiManager;

            playerLocomotion.rigidbody.velocity = Vector3.zero; //Stops the player from moving whilst picking up item
            playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.PickUpName], true); //Plays the animation of looting the item
        }
    }

}
