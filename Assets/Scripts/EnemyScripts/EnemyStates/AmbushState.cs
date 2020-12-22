using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class AmbushState : State
    {
        [Header("Persue Target State", order = 0)]
        [Header("Possible After States", order = 1)]
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        [Header("Ambush Settings", order = 1)]
        public bool isSleeping;
        public float detectionRadius = 2.5f;
        public string sleepAnimation;
        public string wakeAnimation;

        [Header("Player Detection Layer", order = 1)]
        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                if (isSleeping && enemyManager.isInteracting == false)
                {
                    enemyAnimationManager.PlayTargetAnimation(sleepAnimation, true);
                }

                #region Handle Target Detection

                Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

                for (int i = 0; i < colliders.Length; i++)
                {
                    CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                    if (characterStats != null)
                    {
                        Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
                        enemyManager.viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                        if (enemyManager.viewableAngle > enemyManager.minimumDetectionAngle && enemyManager.viewableAngle < enemyManager.maximumDetectionAngle)
                        {
                            enemyManager.currentTarget = characterStats;
                            isSleeping = false;
                            enemyAnimationManager.PlayTargetAnimation(wakeAnimation, true);
                        }
                    }
                }

                #endregion

                #region Handle State Change

                if (enemyManager.currentTarget != null)
                {
                    return pursueTargetState;
                }
                else
                {
                    return this;
                }

                #endregion
            }
            else
            {
                return deathState;
            }
        }
    }
}