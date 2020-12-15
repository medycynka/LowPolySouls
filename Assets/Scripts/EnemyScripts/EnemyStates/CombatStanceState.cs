using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                //Check for attack range
                //potentially circle player or walk around them
                //if in attack range return attack State
                //if we are in a cool down after attacking, return this state and continue circling player
                //if the player runs out of range return the pursuetarget state
                enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
                {
                    return attackState;
                }
                else if (enemyManager.distanceFromTarget > enemyManager.enemyLocomotionManager.stoppingDistance)
                {
                    return pursueTargetState;
                }
                else
                {
                    enemyManager.enemyLocomotionManager.StopMoving();

                    return this;
                }
            }
            else
            {
                return deathState;
            }
        }
    }

}