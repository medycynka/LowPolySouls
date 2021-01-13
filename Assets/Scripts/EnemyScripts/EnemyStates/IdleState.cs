using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class IdleState : State
    {
        [Header("Persue Target State", order = 0)]
        [Header("Possible After States", order = 1)]
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        [Header("Player Detection Layer", order = 1)]
        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                if (!enemyManager.shouldFollowTarget)
                {
                    enemyManager.enemyLocomotionManager.StopMoving();
                }

                #region Handle Enemy Target Detection
                Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
                for (int i = 0; i < colliders.Length; i++)
                {
                    CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                    if (characterStats != null)
                    {
                        //CHECK FOR TEAM ID

                        Vector3 targetDirection = characterStats.transform.position - transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                        if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                        {
                            enemyManager.currentTarget = characterStats;
                        }
                    }
                }
                #endregion

                #region Handle Switching To Next State
                if (enemyManager.currentTarget != null)
                {
                    enemyManager.shouldFollowTarget = true;

                    return pursueTargetState;
                }
                
                return this;
                #endregion
            }
            
            return deathState;
        }
    }

}