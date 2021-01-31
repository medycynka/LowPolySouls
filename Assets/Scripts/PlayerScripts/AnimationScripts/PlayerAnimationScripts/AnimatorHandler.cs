using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class AnimatorHandler : AnimationManager
    {
        private PlayerManager playerManager;
        private PlayerLocomotion playerLocomotion;

        public bool canRotate;
        
        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();

            StaticAnimatorIds.AnimationIds = new Dictionary<string, int>
            {
                {StaticAnimatorIds.VerticalName, Animator.StringToHash(StaticAnimatorIds.VerticalName)},
                {StaticAnimatorIds.HorizontalName, Animator.StringToHash(StaticAnimatorIds.HorizontalName)},
                {StaticAnimatorIds.IsInteractingName, Animator.StringToHash(StaticAnimatorIds.IsInteractingName)},
                {StaticAnimatorIds.CanDoComboName, Animator.StringToHash(StaticAnimatorIds.CanDoComboName)},
                {StaticAnimatorIds.IsInvulnerableName, Animator.StringToHash(StaticAnimatorIds.IsInvulnerableName)},
                {StaticAnimatorIds.IsDeadName, Animator.StringToHash(StaticAnimatorIds.IsDeadName)},
                {StaticAnimatorIds.IsInAirName, Animator.StringToHash(StaticAnimatorIds.IsInAirName)},
                {StaticAnimatorIds.IsUsingLeftHandName, Animator.StringToHash(StaticAnimatorIds.IsUsingLeftHandName)},
                {StaticAnimatorIds.IsUsingRightHandName, Animator.StringToHash(StaticAnimatorIds.IsUsingRightHandName)},
                {StaticAnimatorIds.EmptyName, Animator.StringToHash(StaticAnimatorIds.EmptyName)},
                {StaticAnimatorIds.StandUpName, Animator.StringToHash(StaticAnimatorIds.StandUpName)},
                {StaticAnimatorIds.SitName, Animator.StringToHash(StaticAnimatorIds.SitName)},
                {StaticAnimatorIds.PickUpName, Animator.StringToHash(StaticAnimatorIds.PickUpName)},
                {StaticAnimatorIds.EstusName, Animator.StringToHash(StaticAnimatorIds.EstusName)},
                {StaticAnimatorIds.UseItemName, Animator.StringToHash(StaticAnimatorIds.UseItemName)},
                {StaticAnimatorIds.RollName, Animator.StringToHash(StaticAnimatorIds.RollName)},
                {StaticAnimatorIds.BackStepName, Animator.StringToHash(StaticAnimatorIds.BackStepName)},
                {StaticAnimatorIds.JumpName, Animator.StringToHash(StaticAnimatorIds.JumpName)},
                {StaticAnimatorIds.FallName, Animator.StringToHash(StaticAnimatorIds.FallName)},
                {StaticAnimatorIds.LandName, Animator.StringToHash(StaticAnimatorIds.LandName)},
                {StaticAnimatorIds.Damage01Name, Animator.StringToHash(StaticAnimatorIds.Damage01Name)},
                {StaticAnimatorIds.Death01Name, Animator.StringToHash(StaticAnimatorIds.Death01Name)},
                {StaticAnimatorIds.FogRemoveName, Animator.StringToHash(StaticAnimatorIds.FogRemoveName)},
                {StaticAnimatorIds.BackStabName, Animator.StringToHash(StaticAnimatorIds.BackStabName)},
                {StaticAnimatorIds.BackStabbedName, Animator.StringToHash(StaticAnimatorIds.BackStabbedName)},
                {StaticAnimatorIds.OhLightAttack01, Animator.StringToHash(StaticAnimatorIds.OhLightAttack01)},
                {StaticAnimatorIds.OhLightAttack02, Animator.StringToHash(StaticAnimatorIds.OhLightAttack02)},
                {StaticAnimatorIds.OhLightAttack03, Animator.StringToHash(StaticAnimatorIds.OhLightAttack03)},
                {StaticAnimatorIds.OhLightAttack04, Animator.StringToHash(StaticAnimatorIds.OhLightAttack04)},
                {StaticAnimatorIds.OhLightAttack05, Animator.StringToHash(StaticAnimatorIds.OhLightAttack05)},
                {StaticAnimatorIds.OhLightAttack06, Animator.StringToHash(StaticAnimatorIds.OhLightAttack06)},
                {StaticAnimatorIds.OhHeavyAttack01, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack01)},
                {StaticAnimatorIds.OhHeavyAttack02, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack02)},
                {StaticAnimatorIds.OhHeavyAttack03, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack03)},
                {StaticAnimatorIds.OhHeavyAttack04, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack04)},
                {StaticAnimatorIds.OhHeavyAttack05, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack05)},
                {StaticAnimatorIds.ThLightAttack01, Animator.StringToHash(StaticAnimatorIds.ThLightAttack01)},
                {StaticAnimatorIds.ThLightAttack02, Animator.StringToHash(StaticAnimatorIds.ThLightAttack02)},
                {StaticAnimatorIds.ThLightAttack03, Animator.StringToHash(StaticAnimatorIds.ThLightAttack03)},
                {StaticAnimatorIds.ThHeavyAttack01, Animator.StringToHash(StaticAnimatorIds.ThHeavyAttack01)},
                {StaticAnimatorIds.ThHeavyAttack02, Animator.StringToHash(StaticAnimatorIds.ThHeavyAttack02)},
                {StaticAnimatorIds.LightPunch01, Animator.StringToHash(StaticAnimatorIds.LightPunch01)},
                {StaticAnimatorIds.HeavyPunch01, Animator.StringToHash(StaticAnimatorIds.HeavyPunch01)},
                {StaticAnimatorIds.OhCombo01, Animator.StringToHash(StaticAnimatorIds.OhCombo01)},
                {StaticAnimatorIds.OhCombo02, Animator.StringToHash(StaticAnimatorIds.OhCombo02)},
                {StaticAnimatorIds.OhHeavyCombo01, Animator.StringToHash(StaticAnimatorIds.OhHeavyCombo01)},
                {StaticAnimatorIds.HealSpell, Animator.StringToHash(StaticAnimatorIds.HealSpell)},
            };
            
            anim.SetBool(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.IsDeadName], false);
            Debug.Log("Player: " + StaticAnimatorIds.AnimationIds[StaticAnimatorIds.IsInteractingName]);
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting, bool isWalking)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting && isWalking)
            {
                isWalking = false;
            }
            
            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }
            else if(isWalking)
            {
                v = 0.5f;
                h = horizontalMovement;
            }

            anim.SetFloat(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.VerticalName], v, 0.1f, Time.deltaTime);
            anim.SetFloat(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.HorizontalName], h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public void EnableCombo()
        {
            anim.SetBool(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.CanDoComboName], true);
        }

        public void DisableCombo()
        {
            anim.SetBool(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.CanDoComboName], false);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.IsInvulnerableName], true);
        }
        
        public void DisableIsInvulnerable()
        {
            anim.SetBool(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.IsInvulnerableName], false);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
            {
                return;
            }

            var delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            var deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            var velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}
