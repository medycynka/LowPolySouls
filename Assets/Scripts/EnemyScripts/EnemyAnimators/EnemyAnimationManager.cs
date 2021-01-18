using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class EnemyAnimationManager : AnimationManager
    {
        EnemyLocomotionManager enemyLocomotionManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
            
            StaticAnimatorIds.EnemyVerticalId = Animator.StringToHash(StaticAnimatorIds.VerticalName);
            StaticAnimatorIds.EnemyHorizontalId = Animator.StringToHash(StaticAnimatorIds.HorizontalName);
            StaticAnimatorIds.EnemyIsInteractingId = Animator.StringToHash(StaticAnimatorIds.IsInteractingName);
            StaticAnimatorIds.EnemyEmptyId = Animator.StringToHash(StaticAnimatorIds.EmptyName);
            StaticAnimatorIds.EnemyDamage01Id = Animator.StringToHash(StaticAnimatorIds.Damage01Name);
            StaticAnimatorIds.EnemyDeath01Id = Animator.StringToHash(StaticAnimatorIds.Death01Name);
            StaticAnimatorIds.EnemyGetUpId = Animator.StringToHash(StaticAnimatorIds.GetUpName);
            StaticAnimatorIds.EnemySleepId = Animator.StringToHash(StaticAnimatorIds.SleepName);
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyLocomotionManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            
            if (delta != 0.0f)
            {
                Vector3 velocity = deltaPosition / delta;
                enemyLocomotionManager.enemyRigidBody.velocity = velocity;
            }
            else
            {
                enemyLocomotionManager.enemyRigidBody.velocity = Vector3.zero;
            }
        }
    }

}
