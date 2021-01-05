using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SP
{
    public class EnemyManager : CharacterManager
    {
        [HideInInspector] public EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimationManager enemyAnimationManager;
        EnemyStats enemyStats;
        EnemyDrops enemyDrops;
        //CapsuleCollider blockingCollider;

        [Header("Manager Properties", order = 1)]
        [Header("Bools", order = 2)]
        public bool isPreformingAction;
        public bool isInteracting;
        public bool shouldDrop = true;
        public bool isAlive = true;
        public bool shouldFollowTarget = false;

        [Header("Current Target", order = 2)]
        public CharacterStats currentTarget;

        [Header("Current State from FSM", order = 2)]
        public State currentState;

        [Header("A.I Settings", order = 2)]
        public float distanceFromTarget;
        public float viewableAngle;
        public float detectionRadius = 15;
        public float maximumAttackRange = 1.5f;
        public float maximumDetectionAngle = 75;
        public float minimumDetectionAngle = -75;

        [Header("Recovery Timer", order = 2)]
        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyDrops = GetComponent<EnemyDrops>();
            //blockingCollider = GetComponentsInChildren<CapsuleCollider>().Where(x => x.gameObject.transform.parent != transform.parent).ToArray()[0];
        }

        private void Update()
        {
            HandleRecoveryTimer();

            isInteracting = enemyAnimationManager.anim.GetBool("isInteracting");
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimationManager);

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
            isAlive = false;
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