using UnityEngine;
using UnityEngine.AI;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager _enemyManager;
        private EnemyAnimationManager _enemyAnimationManager;

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
            _enemyManager = GetComponent<EnemyManager>();
            _enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
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
            if (_enemyManager.isPreformingAction)
            {
                _enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f, Time.deltaTime);

                return;
            }

            Vector3 targetDirection = _enemyManager.currentTarget.transform.position - _enemyManager.transform.position;
            _enemyManager.distanceFromTarget = Vector3.Distance(_enemyManager.currentTarget.transform.position, transform.position);
            _enemyManager.viewableAngle = Vector3.Angle(targetDirection, _enemyManager.transform.forward);

            if (_enemyManager.distanceFromTarget > stoppingDistance)
            {
                _enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 1, 0.1f, Time.deltaTime);
            }
            else if (_enemyManager.distanceFromTarget <= stoppingDistance)
            {
                StopMoving();
            }

            HandleRotateTowardsTarget();
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        public void StopMoving()
        {
            _enemyManager.distanceFromTarget = 0;
            _enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f, Time.deltaTime);
        }

        public void HandleRotateTowardsTarget()
        {
            if (_enemyManager.isPreformingAction)
            {
                Vector3 direction = _enemyManager.currentTarget.transform.position - transform.position;
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
                navmeshAgent.SetDestination(_enemyManager.currentTarget.transform.position);
                enemyRigidBody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(_enemyManager.transform.rotation, navmeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }
    }
}