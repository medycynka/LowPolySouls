﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("Locomotion Manager", order = 0)]
        [Header("Camera", order = 1)]
        public CameraHandler cameraHandler;

        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        PlayerStats playerStats;
        CapsuleCollider playerCollider;

        [Header("Move Direction", order = 1)]
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        [Header("Player Rigidbody", order = 1)]
        public new Rigidbody rigidbody;

        [Header("Ground & Air Detection Stats", order = 1)]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats", order = 1)]
        public float movementSpeed = 5;
        public float walkingSpeed = 1;
        public float sprintSpeed = 7;
        public float rotationSpeed = 10;
        public float fallingSpeed = 80;
        public float jumpMult = 10;

        [Header("Next Jump Cooldown", order = 1)]
        public float nextJump = 2.0f;

        [Header("Stamina Costs", order = 1)]
        public float rollStaminaCost = 10;
        public float sprintStaminaCost = 5;

        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            playerStats = GetComponent<PlayerStats>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerCollider = GetComponent<CapsuleCollider>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11 | 1 << 14 | 1 << 20 | 1 << 21 | 1 << 22);

        }

        #region Movement
        private Vector3 normalVector;
        private Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            if (inputHandler.lockOnFlag)
            {
                if (inputHandler.sprintFlag || inputHandler.rollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                    targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                Vector3 targetDir = Vector3.zero;

                targetDir = cameraObject.forward * inputHandler.vertical;
                targetDir += cameraObject.right * inputHandler.horizontal;
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

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
            {
                return;
            }

            if (playerManager.isInteracting)
            {
                return;
            }

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (playerStats.currentStamina > 0 && inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
            {
                playerManager.isSprinting = true;
                moveDirection *= sprintSpeed;
            }
            else
            {
                if (inputHandler.moveAmount < 0.5)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.anim.GetBool(StaticAnimatorIds.IsInteractingId))
            {
                return;
            }

            if (playerStats.currentStamina > 0)
            {
                if (inputHandler.rollFlag)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;

                    if (inputHandler.moveAmount > 0)
                    {
                        animatorHandler.PlayTargetAnimation(StaticAnimatorIds.RollId, true);
                        moveDirection.y = 0;
                        Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = rollRotation;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation(StaticAnimatorIds.BackStepId, true);
                    }

                    playerStats.TakeStaminaDamage(rollStaminaCost);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidbody.velocity = Vector3.down * (fallingSpeed * 0.05f) + moveDirection * (fallingSpeed * 0.01f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, Vector3.down * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, Vector3.down, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        //Debug.Log("You were in the air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation(StaticAnimatorIds.LandId, true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation(StaticAnimatorIds.EmptyId, false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation(StaticAnimatorIds.FallId, true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }

        }

        public void HandleJumping(float delta)
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (inputHandler.jump_Input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    nextJump -= delta;

                    if (nextJump <= 0)
                    {
                        //playerManager.shouldAddJumpForce = true;
                        StartCoroutine(ResizeCollider());
                        animatorHandler.PlayTargetAnimation(StaticAnimatorIds.JumpId, true);
                        Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = jumpRotation;
                    }
                }
            }
        }

        private IEnumerator ResizeCollider()
        {
            yield return CoroutineYielder.jumpFirstWaiter;

            playerCollider.center = Vector3.up * 1.25f;
            playerCollider.height = 1f;

            yield return CoroutineYielder.jumpSecondWaiter;

            playerCollider.center = Vector3.up;
            playerCollider.height = 1.5f;
        }

        public void AddJumpForce(float delta, bool reverse)
        {
            if (reverse)
            {
                if (inputHandler.sprintFlag)
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
                if (inputHandler.sprintFlag)
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