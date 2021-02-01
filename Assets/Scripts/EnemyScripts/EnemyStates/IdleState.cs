using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
{

    public class IdleState : State
    {
        [Header("Persue Target State", order = 0)]
        [Header("Possible After States", order = 1)]
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        [Header("Player Detection Layer", order = 1)]
        public LayerMask detectionLayer;

        private Collider[] detectPlayer = new Collider[2];

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                if (!enemyManager.shouldFollowTarget)
                {
                    enemyManager.enemyLocomotionManager.StopMoving();
                }

                #region Handle Enemy Target Detection
                int detectLength = Physics.OverlapSphereNonAlloc(transform.position, enemyManager.detectionRadius, detectPlayer, detectionLayer);
                
                for (int i = 0; i < detectLength; i++)
                {
                    CharacterStats characterStats = detectPlayer[i].transform.GetComponent<CharacterStats>();
                    
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