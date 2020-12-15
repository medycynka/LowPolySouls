using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class EnemyManager : CharacterManager
    {
        public EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;
        EnemyDrops enemyDrops;

        public bool isPreformingAction;
        public bool shouldDrop = true;
        public bool isAlive = true;
        public bool shouldFollowTarget = false;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public CharacterStats currentTarget;
        public State currentState;

        [Header("A.I Settings")]
        public float distanceFromTarget;
        public float viewableAngle;
        public float detectionRadius = 15;
        public float maximumAttackRange = 1.5f;
        public float maximumDetectionAngle = 75;
        public float minimumDetectionAngle = -75;

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyDrops = GetComponent<EnemyDrops>();
        }

        private void Update()
        {
            HandleRecoveryTimer();
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }
        }

        public void HandleDeath()
        {
            enemyStats.currentHealth = 0;
            enemyStats.animator.Play("Dead_01");
            enemyStats.playerStats.soulsAmount += enemyStats.soulsGiveAmount;
            enemyStats.playerStats.uiManager.currentSoulsAmount.text = enemyStats.playerStats.soulsAmount.ToString();
            Destroy(enemyStats.enemyObject, 5.0f);

            if (shouldDrop)
            {
                enemyDrops.DropPickUp();
                shouldDrop = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.blue;
            Quaternion leftRayRotation = Quaternion.AngleAxis(minimumDetectionAngle, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(maximumDetectionAngle, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.DrawRay(transform.position, leftRayDirection * detectionRadius);
            Gizmos.DrawRay(transform.position, rightRayDirection * detectionRadius);
        }
    }
}