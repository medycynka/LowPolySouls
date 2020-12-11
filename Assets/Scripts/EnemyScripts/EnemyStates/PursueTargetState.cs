using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP {

    public class PursueTargetState : State
    {
        public IdleState idleState;
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                //Chase the target
                //If within attack range, return combat stance state
                //if target is out of range, return this state and continue to chase target
                if(enemyManager.shouldFollowTarget)
                {
                    enemyManager.enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                    if (enemyManager.enemyLocomotionManager.distanceFromTarget <= enemyManager.detectionRadius)
                    {
                        if (enemyManager.enemyLocomotionManager.distanceFromTarget <= enemyManager.enemyLocomotionManager.stoppingDistance)
                        {
                            enemyManager.enemyLocomotionManager.StopMoving();

                            return this;
                        }

                        enemyManager.enemyLocomotionManager.HandleMoveToTarget();

                        return this;
                    }
                    else
                    {
                        //enemyManager.enemyLocomotionManager.OutOfRangeTargetStop();
                        enemyManager.currentTarget = null;
                        enemyManager.shouldFollowTarget = false;

                        return idleState;
                    }
                }
                else
                {
                    return idleState;
                }
            }
            else
            {
                return deathState;
            }
        }
    }

}