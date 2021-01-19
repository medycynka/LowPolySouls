using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SP
{

    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager enemyManager;
        private EnemyAnimationManager enemyAnimationManager;

        [Header("Locomotion Manager", order = 0)]
        [Header("Components", order = 1)]
        public NavMeshAgent navmeshAgent;
        public NavMeshObstacle navMeshBlocker;
        public Rigidbody enemyRigidBody;

        [Header("A.I Movement Stats", order = 1)]
        public float stoppingDistance = 1.25f;
        public float rotationSpeed = 100;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshBlocker = GetComponent<NavMeshObstacle>();
            enemyRigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navmeshAgent.enabled = false;
            enemyRigidBody.isKinematic = false;
        }

        public void HandleMoveToTarget()
        {
            if (enemyManager.isPreformingAction)
            {
                enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.EnemyVerticalId, 0, 0.1f, Time.deltaTime);

                return;
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
            enemyManager.viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (enemyManager.distanceFromTarget > stoppingDistance)
            {
                enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.EnemyVerticalId, 1, 0.1f, Time.deltaTime);
            }
            else if (enemyManager.distanceFromTarget <= stoppingDistance)
            {
                StopMoving();
            }

            HandleRotateTowardsTarget();
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        public void StopMoving()
        {
            enemyManager.distanceFromTarget = 0;
            enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.EnemyVerticalId, 0, 0.1f, Time.deltaTime);
        }

        public void HandleRotateTowardsTarget()
        {
            if (enemyManager.isPreformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }
            //Rotate with pathfinding on navmesh -> make A*?
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyRigidBody.velocity;

                navmeshAgent.enabled = true;
                navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyRigidBody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, navmeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }
    }

}