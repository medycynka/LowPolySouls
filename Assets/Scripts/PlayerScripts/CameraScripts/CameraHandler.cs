using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SP
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("Camera Handler", order = 0)]
        [Header("Player Components", order = 1)]
        public InputHandler inputHandler;
        public PlayerManager playerManager;

        [Header("Look At Targets", order = 1)]
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;

        [Header("Masks", order = 1)]
        public LayerMask ignoreLayers;
        public LayerMask environmentLayer;

        [Header("Camera Basic Properties", order = 1)]
        public Vector3 cameraFollowVelocity = Vector3.zero;
        public float lookSpeed = 0.025f;
        public float followSpeed = 0.5f;
        public float pivotSpeed = 0.03f;
        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        [Header("Camera Detection Properties", order = 1)]
        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffSet = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        [Header("Camera Lock-on Properties", order = 1)]
        public Transform currentLockOnTarget;
        [SerializeField] List<CharacterManager> availableTargets = new List<CharacterManager>();
        public Transform nearestLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        public float maximumLockOnDistance = 20;

        private const string environmentTag = "Environment";
        private LayerMask lockOnLayer;
        
        public Collider[] colliders;
        private void Awake()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 5 | 1 << 8 | 1 << 9 | 1 << 14 | 1 << 20 | 1 << 21);
            targetTransform = playerManager.transform;
            environmentLayer = LayerMask.NameToLayer(environmentTag);
            lockOnLayer = (1 << LayerMask.NameToLayer(environmentTag) | 1 << LayerMask.NameToLayer("Enemy"));
        }

        public void FollowTarget(float delta)
        {
            var newTargetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = newTargetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                var rotation = Vector3.zero;
                rotation.y = lookAngle;
                var targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                if (currentLockOnTarget == null)
                {
                    return;
                }
                
                var dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                var targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                var eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            var direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out var hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                var dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffSet);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            var shortestDistance = Mathf.Infinity;
            var shortestDistanceOfLeftTarget = Mathf.Infinity;
            var shortestDistanceOfRightTarget = Mathf.Infinity;
            
            colliders = Physics.OverlapSphere(targetTransform.position, 26, lockOnLayer);
            //Debug.Log(colliders.Length);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out CharacterManager character))
                {
                    var lockTargetDirection = character.transform.position - targetTransform.position;
                    var distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    var viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if (character.transform.root == targetTransform.transform.root || !(viewableAngle > -50) || !(viewableAngle < 50) || !(distanceFromTarget <= maximumLockOnDistance))
                    {
                        continue;
                    }

                    if (!Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        continue;
                    }

                    //Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position, Color.green, 10f);

                    if (hit.transform.gameObject.CompareTag(environmentTag) || hit.transform.gameObject.layer == environmentLayer)
                    {
                        //Cannot lock onto target, object in the way
                    }
                    else
                    {
                        availableTargets.Add(character);
                    }
                }
            }

            if (currentLockOnTarget != null)
            {
                return;
            }

            for (int i = 0; i < availableTargets.Count; i++)
            {
                var availableTarget = availableTargets[i];
                var distanceFromTarget = Vector3.Distance(targetTransform.position, availableTarget.transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTarget.lockOnTransform;
                }

                if (!inputHandler.lockOnFlag)
                {
                    continue;
                }

                var relativeEnemyPosition =
                    currentLockOnTarget.InverseTransformPoint(availableTarget.transform.position);
                var distanceFromLeftTarget =
                    currentLockOnTarget.transform.position.x - availableTarget.transform.position.x;
                var distanceFromRightTarget =
                    currentLockOnTarget.transform.position.x + availableTarget.transform.position.x;

                if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTarget.lockOnTransform;
                }

                if (!(relativeEnemyPosition.x < 0.00) || !(distanceFromRightTarget < shortestDistanceOfRightTarget))
                {
                    continue;
                }

                shortestDistanceOfRightTarget = distanceFromRightTarget;
                rightLockTarget = availableTarget.lockOnTransform;
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maximumLockOnDistance);
        }
    }
}