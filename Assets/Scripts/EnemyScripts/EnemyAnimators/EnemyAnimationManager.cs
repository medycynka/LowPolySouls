using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;


namespace SzymonPeszek.EnemyScripts.Animations
{
    public class EnemyAnimationManager : AnimationManager
    {
        private EnemyLocomotionManager _enemyLocomotionManager;
        private EnemyManager _enemyManager;
        private EnemyStats _enemyStats;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            _enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
            _enemyManager = GetComponentInParent<EnemyManager>();
            _enemyStats = GetComponentInParent<EnemyStats>();

            StaticAnimatorIds.enemyAnimationIds = new Dictionary<string, int>
            {
                {StaticAnimatorIds.VerticalName, Animator.StringToHash(StaticAnimatorIds.VerticalName)},
                {StaticAnimatorIds.HorizontalName, Animator.StringToHash(StaticAnimatorIds.HorizontalName)},
                {StaticAnimatorIds.IsInteractingName, Animator.StringToHash(StaticAnimatorIds.IsInteractingName)},
                {StaticAnimatorIds.IsDeadName, Animator.StringToHash(StaticAnimatorIds.IsDeadName)},
                {StaticAnimatorIds.CanRotateName, Animator.StringToHash(StaticAnimatorIds.CanRotateName)},
                {StaticAnimatorIds.EmptyName, Animator.StringToHash(StaticAnimatorIds.EmptyName)},
                {StaticAnimatorIds.Damage01Name, Animator.StringToHash(StaticAnimatorIds.Damage01Name)},
                {StaticAnimatorIds.Death01Name, Animator.StringToHash(StaticAnimatorIds.Death01Name)},
                {StaticAnimatorIds.GetUpName, Animator.StringToHash(StaticAnimatorIds.GetUpName)},
                {StaticAnimatorIds.SleepName, Animator.StringToHash(StaticAnimatorIds.SleepName)},
                {StaticAnimatorIds.BackStabName, Animator.StringToHash(StaticAnimatorIds.BackStabName)},
                {StaticAnimatorIds.BackStabbedName, Animator.StringToHash(StaticAnimatorIds.BackStabbedName)},
                {StaticAnimatorIds.LayDownName, Animator.StringToHash(StaticAnimatorIds.LayDownName)},
                {StaticAnimatorIds.EnemyMaceAttack01, Animator.StringToHash(StaticAnimatorIds.EnemyMaceAttack01)},
                {StaticAnimatorIds.EnemyMaceAttack02, Animator.StringToHash(StaticAnimatorIds.EnemyMaceAttack02)},
                {StaticAnimatorIds.EnemyMaceAttack03, Animator.StringToHash(StaticAnimatorIds.EnemyMaceAttack03)},
                {StaticAnimatorIds.EnemyStaffAttack01, Animator.StringToHash(StaticAnimatorIds.EnemyStaffAttack01)},
                {StaticAnimatorIds.EnemyStaffAttack02, Animator.StringToHash(StaticAnimatorIds.EnemyStaffAttack02)},
                {StaticAnimatorIds.EnemySpearAttack01, Animator.StringToHash(StaticAnimatorIds.EnemySpearAttack01)},
                {StaticAnimatorIds.EnemySpearAttack02, Animator.StringToHash(StaticAnimatorIds.EnemySpearAttack02)},
                {StaticAnimatorIds.EnemySwordAttack01, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack01)},
                {StaticAnimatorIds.EnemySwordAttack02, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack02)},
                {StaticAnimatorIds.EnemySwordAttack03, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack03)},
                {StaticAnimatorIds.EnemySwordAttack04, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack04)},
                {StaticAnimatorIds.EnemySwordAttack05, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack05)},
                {StaticAnimatorIds.EnemySwordAttack06, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack06)},
            };
            
            anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsDeadName], false);
            Debug.Log("Enemy: " + StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInteractingName]);
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyLocomotionManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            
            if (delta != 0.0f)
            {
                Vector3 velocity = deltaPosition / delta;
                _enemyLocomotionManager.enemyRigidBody.velocity = velocity;
            }
            else
            {
                _enemyLocomotionManager.enemyRigidBody.velocity = Vector3.zero;
            }
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            _enemyStats.TakeDamage(_enemyManager.pendingCriticalDamage, false, true);
            _enemyManager.pendingCriticalDamage = 0.0f;
        }
    }

}
