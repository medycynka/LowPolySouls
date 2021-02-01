using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
{

    public class CombatStanceState : State
    {
        [Header("Combat Stance State", order = 0)]
        [Header("Possible After States", order = 1)]
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                enemyManager.enemyLocomotionManager.HandleRotateTowardsTarget();

                if (enemyManager.isPreformingAction)
                {
                    enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f, Time.deltaTime);
                }

                enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
                {
                    return attackState;
                }
                
                if (enemyManager.distanceFromTarget > enemyManager.enemyLocomotionManager.stoppingDistance)
                {
                    return pursueTargetState;
                }
                
                enemyManager.enemyLocomotionManager.StopMoving();

                return this;
            }
            
            return deathState;
        }
    }

}