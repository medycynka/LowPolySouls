using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
{
    /// <summary>
    /// Class representing idle state
    /// </summary>
    public class IdleState : State
    {
        [Header("Pursue Target State", order = 0)]
        [Header("Possible After States", order = 1)]
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        [Header("Player Detection Layer", order = 1)]
        public LayerMask detectionLayer;

        private Collider[] detectPlayer = new Collider[2];

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
                #region Handle Enemy Target Detection
                int detectLength = Physics.OverlapSphereNonAlloc(enemyManager.enemyTransform.position, enemyManager.detectionRadius, detectPlayer, detectionLayer);

                for (int i = 0; i < detectLength; i++)
                {
                    CharacterStats characterStats = detectPlayer[i].transform.GetComponent<CharacterStats>();

                    if (characterStats != null)
                    {
                        Vector3 targetDirection = characterStats.transform.position - enemyManager.enemyTransform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.enemyTransform.forward);

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
                    return pursueTargetState;
                }
                
                return this;
                #endregion
            }
            
            return deathState;
        }
    }

}