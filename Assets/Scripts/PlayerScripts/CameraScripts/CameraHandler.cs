using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.BaseClasses;


namespace SzymonPeszek.PlayerScripts.CameraManager
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
        private Transform _myTransform;
        private Vector3 _cameraTransformPosition;

        [Header("Masks", order = 1)]
        public LayerMask ignoreLayers;
        public LayerMask environmentLayer;

        [Header("Camera Basic Properties", order = 1)]
        public Vector3 cameraFollowVelocity = Vector3.zero;
        public float lookSpeed = 0.025f;
        public float followSpeed = 0.5f;
        public float pivotSpeed = 0.03f;
        private float _targetPosition;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;
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
        private readonly List<CharacterManager> _availableTargets = new List<CharacterManager>();
        public Transform nearestLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        public float maximumLockOnDistance = 20;
        private const string EnvironmentTag = "Environment";
        private LayerMask _lockOnLayer;
        private RaycastHit _hit;
        private float _LockOnAngle = 120f;
        public Collider[] colliders;
        private int _collidersSize = 512;
        private int _collidersPrevSize = 512;
        
        private void Awake()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            _myTransform = transform;
            _defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 5 | 1 << 8 | 1 << 9 | 1 << 14 | 1 << 20 | 1 << 21 | 1 << 23);
            targetTransform = playerManager.transform;
            environmentLayer = 1 << LayerMask.NameToLayer(EnvironmentTag);
            _lockOnLayer = (1 << LayerMask.NameToLayer(EnvironmentTag) | 1 << LayerMask.NameToLayer("Enemy"));
            colliders = new Collider[_collidersSize];
        }

        public void FollowTarget(float delta)
        {
            var newTargetPosition = Vector3.SmoothDamp(_myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            _myTransform.position = newTargetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                _lookAngle += (mouseXInput * lookSpeed) / delta;
                _pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivot, maximumPivot);

                var rotation = Vector3.zero;
                rotation.y = _lookAngle;
                var targetRotation = Quaternion.Euler(rotation);
                _myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = _pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                if (currentLockOnTarget == null)
                {
                    return;
                }

                Vector3 currentLockOnTargetPosition = currentLockOnTarget.position;
                Vector3 dir = currentLockOnTargetPosition - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTargetPosition - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        private void HandleCameraCollisions(float delta)
        {
            _targetPosition = _defaultPosition;
            var direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out var hit, Mathf.Abs(_targetPosition), ignoreLayers))
            {
                var dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                _targetPosition = -(dis - cameraCollisionOffSet);
            }

            if (Mathf.Abs(_targetPosition) < minimumCollisionOffset)
            {
                _targetPosition = -minimumCollisionOffset;
            }

            _cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, _targetPosition, delta / 0.2f);
            cameraTransform.localPosition = _cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            var shortestDistance = Mathf.Infinity;
            var shortestDistanceOfLeftTarget = Mathf.Infinity;
            var shortestDistanceOfRightTarget = Mathf.Infinity;
            int collidersLenght = Physics.OverlapSphereNonAlloc(targetTransform.position, maximumLockOnDistance, colliders, _lockOnLayer);

            if (2 * collidersLenght < _collidersSize)
            {
                _collidersPrevSize = _collidersSize;
                _collidersSize /= 2;
            }
            else if (collidersLenght > _collidersSize)
            {
                _collidersPrevSize = _collidersSize;
                _collidersSize *= 2;
            }

            Vector3 currentTargetPosition = targetTransform.position;
            
            for (int i = 0; i < collidersLenght; i++)
            {
                if (colliders[i].TryGetComponent(out CharacterManager character))
                {
                    var lockTargetDirection = character.transform.position - targetTransform.position;
                    var distanceFromTarget = Vector3.Distance(currentTargetPosition, character.characterTransform.position);
                    var viewableAngle = Vector3.Angle(lockTargetDirection, playerManager.gameObject.transform.forward);

                    if (character.transform.root == targetTransform.transform.root || !(viewableAngle > -_LockOnAngle) || !(viewableAngle < _LockOnAngle) || !(distanceFromTarget <= maximumLockOnDistance))
                    {
                        continue;
                    }

                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out _hit))
                    {
                        if (_hit.transform.gameObject.CompareTag(EnvironmentTag) ||
                            _hit.transform.gameObject.layer == environmentLayer)
                        {
                            //Cannot lock onto target, object in the way
                        }
                        else
                        {
                            _availableTargets.Add(character);
                        }
                    }
                }
            }

            if (currentLockOnTarget != null)
            {
                return;
            }

            for (int i = 0; i < _availableTargets.Count; i++)
            {
                var availableTarget = _availableTargets[i];
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

                Vector3 availableTargetPosition = availableTarget.characterTransform.position;
                Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargetPosition);
                Vector3 currentLockOnTargetPosition = currentLockOnTarget.position;
                float distanceFromLeftTarget = currentLockOnTargetPosition.x - availableTargetPosition.x;
                float distanceFromRightTarget = currentLockOnTargetPosition.x + availableTargetPosition.x;

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

            if (_collidersPrevSize != _collidersSize)
            {
                colliders = new Collider[collidersLenght];
            }
        }

        public void ClearLockOnTargets()
        {
            _availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            cameraPivotTransform.localPosition = currentLockOnTarget != null ? Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime) : Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maximumLockOnDistance);
        }
    }
}