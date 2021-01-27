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
            
            StaticAnimatorIds.VerticalId = Animator.StringToHash(StaticAnimatorIds.VerticalName);
            StaticAnimatorIds.HorizontalId = Animator.StringToHash(StaticAnimatorIds.HorizontalName);
            StaticAnimatorIds.IsInteractingId = Animator.StringToHash(StaticAnimatorIds.IsInteractingName);
            StaticAnimatorIds.CanDoComboId = Animator.StringToHash(StaticAnimatorIds.CanDoComboName);
            StaticAnimatorIds.IsInvulnerableId = Animator.StringToHash(StaticAnimatorIds.IsInvulnerableName);
            StaticAnimatorIds.IsDeadId = Animator.StringToHash(StaticAnimatorIds.IsDeadName);
            StaticAnimatorIds.IsInAirId = Animator.StringToHash(StaticAnimatorIds.IsInAirName);
            StaticAnimatorIds.IsUsingLeftHandId = Animator.StringToHash(StaticAnimatorIds.IsUsingLeftHandName);
            StaticAnimatorIds.IsUsingRightHandId = Animator.StringToHash(StaticAnimatorIds.IsUsingRightHandName);
            StaticAnimatorIds.EmptyId = Animator.StringToHash(StaticAnimatorIds.EmptyName);
            StaticAnimatorIds.StandUpId = Animator.StringToHash(StaticAnimatorIds.StandUpName);
            StaticAnimatorIds.SitId = Animator.StringToHash(StaticAnimatorIds.SitName);
            StaticAnimatorIds.PickUpId = Animator.StringToHash(StaticAnimatorIds.PickUpName);
            StaticAnimatorIds.EstusId = Animator.StringToHash(StaticAnimatorIds.EstusName);
            StaticAnimatorIds.UseItemId = Animator.StringToHash(StaticAnimatorIds.UseItemName);
            StaticAnimatorIds.RollId = Animator.StringToHash(StaticAnimatorIds.RollName);
            StaticAnimatorIds.BackStepId = Animator.StringToHash(StaticAnimatorIds.BackStepName);
            StaticAnimatorIds.JumpId = Animator.StringToHash(StaticAnimatorIds.JumpName);
            StaticAnimatorIds.FallId = Animator.StringToHash(StaticAnimatorIds.FallName);
            StaticAnimatorIds.LandId = Animator.StringToHash(StaticAnimatorIds.LandName);
            StaticAnimatorIds.Damage01Id = Animator.StringToHash(StaticAnimatorIds.Damage01Name);
            StaticAnimatorIds.Death01Id = Animator.StringToHash(StaticAnimatorIds.Death01Name);
            StaticAnimatorIds.FogRemoveId = Animator.StringToHash(StaticAnimatorIds.FogRemoveName);
            StaticAnimatorIds.BackStabId = Animator.StringToHash(StaticAnimatorIds.BackStabName);
            StaticAnimatorIds.BackStabbedId = Animator.StringToHash(StaticAnimatorIds.BackStabbedName);
            StaticAnimatorIds.LayDownId = Animator.StringToHash(StaticAnimatorIds.LayDownName);
            anim.SetBool(StaticAnimatorIds.IsDeadId, false);
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
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

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(StaticAnimatorIds.VerticalId, v, 0.1f, Time.deltaTime);
            anim.SetFloat(StaticAnimatorIds.HorizontalId, h, 0.1f, Time.deltaTime);
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
            anim.SetBool(StaticAnimatorIds.CanDoComboId, true);
        }

        public void DisableCombo()
        {
            anim.SetBool(StaticAnimatorIds.CanDoComboId, false);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool(StaticAnimatorIds.IsInvulnerableId, true);
        }
        
        public void DisableIsInvulnerable()
        {
            anim.SetBool(StaticAnimatorIds.IsInvulnerableId, false);
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
