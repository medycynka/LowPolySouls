using System.Collections;
using UnityEngine;
using SzymonPeszek.PlayerScripts.CameraManager;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;


namespace SzymonPeszek.PlayerScripts
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("Locomotion Manager", order = 0)]
        [Header("Camera", order = 1)]
        public CameraHandler cameraHandler;

        private PlayerManager _playerManager;
        private Transform _cameraObject;
        private InputHandler _inputHandler;
        private PlayerStats _playerStats;
        private CapsuleCollider _playerCollider;
        private FootIkManager _footIkManager;

        [Header("Move Direction", order = 1)]
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimatorHandler playerAnimatorHandler;

        [Header("Player Rigidbody", order = 1)]
        public new Rigidbody rigidbody;

        [Header("Ground & Air Detection Stats", order = 1)]
        [SerializeField]
        private float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        private float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        private float groundDirectionRayDistance = 0.2f;
        [SerializeField] private LayerMask _ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats", order = 1)]
        public float movementSpeed = 5;
        public float walkingSpeed = 1;
        public float sprintSpeed = 7;
        public float rotationSpeed = 16;
        public float fallingSpeed = 80;
        public float jumpMult = 10;

        [Header("Next Jump Cooldown", order = 1)]
        public float nextJump = 2.0f;

        [Header("Stamina Costs", order = 1)]
        public float rollStaminaCost = 10;
        public float sprintStaminaCost = 5;
        
        private RaycastHit _hit;

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            _playerStats = GetComponent<PlayerStats>();
            playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            _playerCollider = GetComponent<CapsuleCollider>();
            _footIkManager = GetComponentInChildren<FootIkManager>();
            
            if (!(Camera.main is null))
            {
                _cameraObject = Camera.main.transform;
            }
            
            if (cameraHandler == null)
            {
                cameraHandler = _cameraObject.GetComponent<CameraHandler>();
            }
            
            myTransform = transform;
            playerAnimatorHandler.Initialize();

            _playerManager.isGrounded = true;
            _ignoreForGroundCheck = ~(1 << 8 | 1 << 11 | 1 << 14 | 1 << 20 | 1 << 21 | 1 << 22 | 1 << 23 | 1 << 24);
        }

        #region Movement
        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        public void HandleRotation(float delta)
        {
            if (playerAnimatorHandler.canRotate)
            {
                if (_inputHandler.lockOnFlag)
                {
                    if (_inputHandler.sprintFlag || _inputHandler.rollFlag)
                    {
                        Vector3 targetDirection = cameraHandler.cameraTransform.forward * _inputHandler.vertical;
                        targetDirection += cameraHandler.cameraTransform.right * _inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation =
                            Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation =
                            Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    Vector3 targetDir = _cameraObject.forward * _inputHandler.vertical;
                    targetDir += _cameraObject.right * _inputHandler.horizontal;
                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                    {
                        targetDir = myTransform.forward;
                    }

                    float rs = rotationSpeed;
                    Quaternion tr = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
                    myTransform.rotation = targetRotation;
                }
            }
        }

        public void HandleMovement(float delta)
        {
            if (_inputHandler.rollFlag)
            {
                return;
            }

            if (_playerManager.isInteracting)
            {
                return;
            }

            moveDirection = _cameraObject.forward * _inputHandler.vertical;
            moveDirection += _cameraObject.right * _inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (_playerStats.currentStamina > 0 && _inputHandler.sprintFlag && _inputHandler.moveAmount > 0.5)
            {
                _playerManager.isSprinting = true;
                moveDirection *= sprintSpeed;
            }
            else
            {
                if (_inputHandler.walkFlag)
                {
                    moveDirection *= walkingSpeed;
                    _playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    _playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
            rigidbody.velocity = projectedVelocity;

            if (_inputHandler.lockOnFlag && _inputHandler.sprintFlag == false)
            {
                playerAnimatorHandler.UpdateAnimatorValues(_inputHandler.vertical, _inputHandler.horizontal, _playerManager.isSprinting, _inputHandler.walkFlag);
            }
            else
            {
                playerAnimatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0, _playerManager.isSprinting, _inputHandler.walkFlag);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (playerAnimatorHandler.anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName]))
            {
                return;
            }

            if (_playerStats.currentStamina > 0)
            {
                if (_inputHandler.rollFlag)
                {
                    moveDirection = _cameraObject.forward * _inputHandler.vertical;
                    moveDirection += _cameraObject.right * _inputHandler.horizontal;

                    if (_inputHandler.moveAmount > 0)
                    {
                        playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.RollName], true);
                        moveDirection.y = 0;
                        Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = rollRotation;
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.BackStepName], true);
                    }

                    _playerStats.TakeStaminaDamage(rollStaminaCost);
                }
            }
        }

        public void HandleFalling(Vector3 moveDir)
        {
            _playerManager.isGrounded = false;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out _hit, 0.4f))
            {
                moveDir = Vector3.zero;
            }

            if (_playerManager.isInAir)
            {
                rigidbody.velocity = Vector3.down * (fallingSpeed * 0.05f) + moveDir * (fallingSpeed * 0.01f);
            }

            Vector3 dir = moveDir;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            _targetPosition = myTransform.position;

            Debug.DrawRay(origin, Vector3.down * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            
            if (Physics.Raycast(origin, Vector3.down, out _hit, minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
            {
                _normalVector = _hit.normal;
                Vector3 tp = _hit.point;
                _playerManager.isGrounded = true;
                _targetPosition.y = tp.y;

                if (_playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        //Debug.Log("You were in the air for " + inAirTimer);
                        playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.LandName], true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.EmptyName], false);
                        inAirTimer = 0;
                    }

                    _playerManager.isInAir = false;
                    _footIkManager.enableFeetIk = true;
                }
            }
            else
            {
                if (_playerManager.isGrounded)
                {
                    _playerManager.isGrounded = false;
                }

                if (_playerManager.isInAir == false)
                {
                    _footIkManager.enableFeetIk = false;
                    
                    if (_playerManager.isInteracting == false)
                    {
                        playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.FallName], true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    _playerManager.isInAir = true;
                }
            }

            if (_playerManager.isInteracting || _inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, _targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = _targetPosition;
            }

        }

        public void HandleJumping(float delta)
        {
            if (_playerManager.isInteracting)
            {
                return;
            }

            if (_inputHandler.jumpInput)
            {
                if (_inputHandler.moveAmount > 0)
                {
                    nextJump -= delta;

                    if (nextJump <= 0)
                    {
                        //playerManager.shouldAddJumpForce = true;
                        StartCoroutine(ResizeCollider());
                        playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.JumpName], true);
                        Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = jumpRotation;
                    }
                }
            }
        }

        private IEnumerator ResizeCollider()
        {
            yield return CoroutineYielder.jumpFirstWaiter;

            _playerCollider.center = Vector3.up * 1.25f;
            _playerCollider.height = 1f;

            yield return CoroutineYielder.jumpSecondWaiter;

            _playerCollider.center = Vector3.up;
            _playerCollider.height = 1.5f;
        }

        public void AddJumpForce(float delta, bool reverse)
        {
            if (reverse)
            {
                if (_inputHandler.sprintFlag)
                {
                    rigidbody.AddForce(moveDirection * (jumpMult * sprintSpeed * 0.1f * delta), ForceMode.Impulse);
                }
                else
                {
                    rigidbody.AddForce(moveDirection * (jumpMult * movementSpeed * 0.2f * delta), ForceMode.Impulse);
                }
            }
            else
            {
                if (_inputHandler.sprintFlag)
                {
                    rigidbody.AddForce(new Vector3(moveDirection.x * jumpMult * sprintSpeed * 0.1f,
                        jumpMult * 5f,
                        moveDirection.z * jumpMult * sprintSpeed * 0.1f) * delta, ForceMode.Impulse);
                }
                else
                {
                    rigidbody.AddForce(new Vector3(moveDirection.x * jumpMult * movementSpeed * 0.2f,
                        jumpMult * 5f,
                        moveDirection.z * jumpMult * movementSpeed * 0.2f) * delta, ForceMode.Impulse);
                }
            }
        }
        #endregion
    }
}