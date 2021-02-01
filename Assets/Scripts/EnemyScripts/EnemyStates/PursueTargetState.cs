using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
{
    public class PursueTargetState : State
    {
        [Header("Persue Target State", order = 0)]
        [Header("Possible After States", order = 1)]
        public IdleState idleState;
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                if(enemyManager.shouldFollowTarget)
                {
                    enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                    if (enemyManager.distanceFromTarget <= enemyManager.detectionRadius)
                    {
                        if (enemyManager.distanceFromTarget <= enemyManager.enemyLocomotionManager.stoppingDistance)
                        {
                            enemyManager.enemyLocomotionManager.StopMoving();

                            return combatStanceState;
                        }

                        enemyManager.enemyLocomotionManager.HandleMoveToTarget();

                        return this;
                    }
                    
                    enemyManager.currentTarget = null;
                    enemyManager.shouldFollowTarget = false;

                    return idleState;
                }
                
                return idleState;
            }
            
            return deathState;
        }
    }

}