using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
{
    /// <summary>
    /// Class representing combat state
    /// </summary>
    public class CombatStanceState : State
    {
        [Header("Combat Stance State", order = 0)]
        [Header("Possible After States", order = 1)]
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        /// <summary>
        /// Use state behaviour
        /// </summary>
        /// <param name="enemyManager">Enemy manager</param>
        /// <param name="enemyStats">Enemy stats</param>
        /// <param name="enemyAnimationManager">Enemy animation manager</param>
        /// <returns>This or next state</returns>
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.enemyTransform.position);
                
                if (enemyManager.isPreformingAction)
                {
                    enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f, Time.deltaTime);
                }

                if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
                {
                    return attackState;
                }
                
                if (distanceFromTarget > enemyManager.maximumAttackRange)
                {
                    return pursueTargetState;
                }

                return this;
            }
            
            return deathState;
        }
    }

}