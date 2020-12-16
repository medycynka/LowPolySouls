using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        public CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;
        PlayerStats playerStats;

        public InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        public bool isInteracting;

        [Header("Heleper bools")]
        public bool shouldRefillHealth = false;
        public bool shouldRefillStamina = false;
        public bool shouldRefillHealthBg = false;
        public bool shouldRefillStaminaBg = false;
        public bool shouldAddJumpForce = false;
        public bool isRestingAtBonfire = false;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        [Header("Respawn")]
        public GameObject quickMoveScreen;
        public GameObject currentSpawnPoint;

        float healhBgRefillTimer = 0.0f;
        float staminaRefillTimer = 0.0f;
        float addJumpForceTimer = 1.25f;

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerStats = GetComponent<PlayerStats>();
        }

        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);

            inputHandler.TickInput(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);

            CheckAllFunctions(delta);

            if (Time.time > playerLocomotion.nextJump)
            {
                playerLocomotion.HandleJumping(delta);
            }
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                if (!inputHandler.inventoryFlag)
                {
                    cameraHandler.FollowTarget(delta);
                    cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
                }

                if(cameraHandler.currentLockOnTarget == null && inputHandler.lockOnFlag)
                {
                    inputHandler.lockOnFlag = false;
                }
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        #region Checking Funkctions
        public void CheckAllFunctions(float delta)
        {
            CheckForJumpForce(delta);
            CheckForInteractableObject();
            FillHealthBarBackGround(delta);
            CheckForHealthRefill();
            CheckForStaminaRefill(delta);
            CheckForSprintStaminaDrain(delta);
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
            {
                if (hit.collider.tag == "Interactable" || hit.collider.tag == "Bonfire")
                {
                    if (hit.collider.tag == "Bonfire")
                    {
                        if (!hit.collider.GetComponent<BonfireManager>().isActivated)
                        {
                            BonfireActivator interactableObject = hit.collider.GetComponent<BonfireActivator>();

                            if (interactableObject != null)
                            {
                                interactableUI.interactableText.text = interactableObject.interactableText;
                                interactableUIGameObject.SetActive(true);

                                if (inputHandler.a_Input)
                                {
                                    interactableObject.Interact(this);
                                    interactableUIGameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            BonfireInteraction interactableObject = hit.collider.GetComponent<BonfireInteraction>();

                            if (interactableObject != null)
                            {
                                if (interactableObject.GetComponent<BonfireManager>().showRestPopUp)
                                {
                                    interactableUI.interactableText.text = interactableObject.interactableText;
                                    interactableUIGameObject.SetActive(true);

                                    if (inputHandler.a_Input)
                                    {
                                        interactableObject.Interact(this);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                        if (interactableObject != null)
                        {
                            interactableUI.interactableText.text = interactableObject.interactableText;
                            interactableUIGameObject.SetActive(true);

                            if (inputHandler.a_Input)
                            {
                                interactableObject.Interact(this);
                            }
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        private void FillHealthBarBackGround(float delta)
        {
            shouldRefillHealthBg = playerStats.healthBar.backgroundSlider.value > playerStats.healthBar.healthBarSlider.value;

            if (shouldRefillHealthBg)
            {
                if(healhBgRefillTimer > 1.5f)
                {
                    playerStats.healthBar.backgroundSlider.value -= playerStats.healthBgRefillAmount * delta;

                    if(playerStats.healthBar.backgroundSlider.value <= playerStats.healthBar.healthBarSlider.value)
                    {
                        playerStats.healthBar.backgroundSlider.value = playerStats.healthBar.healthBarSlider.value;
                    }
                }
                else
                {
                    healhBgRefillTimer += delta;
                }
            }
        }

        private void CheckForHealthRefill()
        {
            if (shouldRefillHealth)
            {
                playerStats.RefillHealth();

                if(playerStats.currentHealth >= playerStats.maxHealth)
                {
                    shouldRefillHealth = false;
                }
            }
        }

        private void CheckForStaminaRefill(float delta)
        {
            shouldRefillStamina = !isInteracting && !inputHandler.comboFlag && !inputHandler.sprintFlag && (playerStats.currentStamina < playerStats.maxStamina);

            if (shouldRefillStamina)
            {
                if (staminaRefillTimer > 1.0f)
                {
                    playerStats.RefillStamina();
                }
                else
                {
                    staminaRefillTimer += delta;
                }
            }
            else
            {
                staminaRefillTimer = 0.0f;
            }
        }

        private void CheckForJumpForce(float delta)
        {
            if (shouldAddJumpForce)
            {
                addJumpForceTimer -= delta;
                playerLocomotion.AddJumpForce(delta, addJumpForceTimer <= 0.5);

                if(addJumpForceTimer <= 0)
                {
                    shouldAddJumpForce = false;
                    addJumpForceTimer = 1.25f;
                }
            }
        }

        private void CheckForSprintStaminaDrain(float delta)
        {
            if (inputHandler.sprintFlag)
            {
                playerStats.TakeStaminaDamage(playerLocomotion.sprintStaminaCost * delta);

                if(playerStats.currentStamina <= 0f)
                {
                    isSprinting = false;
                }
            }
        }
        #endregion
    }
}