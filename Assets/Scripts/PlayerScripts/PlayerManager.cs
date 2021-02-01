using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.CameraManager;
using SzymonPeszek.GameUI;
using SzymonPeszek.Misc;
using SzymonPeszek.Items.Bonfire;
using SzymonPeszek.Environment.Areas;


namespace SzymonPeszek.PlayerScripts
{
    public class PlayerManager : CharacterManager
    {
        private InputHandler inputHandler;
        private AnimatorHandler animatorHandler;
        [Header("Player Components", order = 1)]
        [Header("Camera Component", order = 2)]
        public CameraHandler cameraHandler;
        private PlayerLocomotion playerLocomotion;
        private PlayerStats playerStats;
        
        [Header("UI", order = 2)]
        public InteractableUI interactableUI;

        [Header("Interactable Objects UI", order = 2)]
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        [Header("Animator Interaction Bool", order = 2)]
        public bool isInteracting;

        [Header("Helper bools", order = 2)]
        public bool shouldRefillHealth = false;
        public bool shouldRefillStamina = false;
        public bool shouldRefillFocus = false;
        public bool shouldRefillHealthBg = false;
        public bool shouldAddJumpForce = false;
        public bool isRestingAtBonfire = false;
        public bool isRemovingFog = false;

        [Header("Player Flags", order = 2)]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;

        [Header("Respawn Places", order = 2)]
        public GameObject quickMoveScreen;
        public GameObject currentSpawnPoint;

        private float healthBgRefillTimer = 0.0f;
        private float staminaRefillTimer = 0.0f;
        private float focusRefillTimer = 0.0f;
        private float addJumpForceTimer = 1.25f;

        private const string bonfireTag = "Bonfire";
        private const string interactableTag = "Interactable";
        private const string fogWallTag = "Fog Wall";
        private LayerMask pickUpLayer;
        private Collider[] interactColliders;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<BackStabCollider>();
        }

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerStats = GetComponent<PlayerStats>();
            pickUpLayer = 1 << LayerMask.NameToLayer("Pick Up");
            interactableUI = FindObjectOfType<InteractableUI>();
            interactColliders = new Collider[8];
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            
            isInteracting = animatorHandler.anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName]);
            canDoCombo = animatorHandler.anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanDoComboName]);
            isUsingRightHand = animatorHandler.anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsUsingRightHandName]);
            isUsingLeftHand = animatorHandler.anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsUsingLeftHandName]);
            isInvulnerable = animatorHandler.anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInvulnerableName]);
            animatorHandler.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInAirName], isInAir);

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
            playerLocomotion.HandleFalling(playerLocomotion.moveDirection);
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rbInput = false;
            inputHandler.rtInput = false;
            inputHandler.dPadUp = false;
            inputHandler.dPadDown = false;
            inputHandler.dPadLeft = false;
            inputHandler.dPadRight = false;
            inputHandler.aInput = false;
            inputHandler.jumpInput = false;
            inputHandler.inventoryInput = false;
            inputHandler.estusQuickSlotUse = false;

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
        private void CheckAllFunctions(float delta)
        {
            CheckForJumpForce(delta);
            CheckForInteractableObject();
            FillHealthBarBackGround(delta);
            CheckForHealthRefill();
            CheckForStaminaRefill(delta);
            CheckForFocusRefill(delta);
            CheckForSprintStaminaDrain(delta);
        }

        private void CheckForInteractableObject()
        {
            int collidersLength = Physics.OverlapSphereNonAlloc(transform.position, 1f, interactColliders, pickUpLayer);
            
            if (collidersLength > 0)
            {
                for (int i = 0; i < collidersLength; i++)
                {
                    if (interactColliders[i].CompareTag(bonfireTag))
                    {
                        BonfireManager bonManager = interactColliders[i].GetComponent<BonfireManager>();

                        if (!bonManager.isActivated)
                        {
                            BonfireActivator interactableObject = bonManager.GetComponent<BonfireActivator>();

                            if (interactableObject != null)
                            {
                                interactableUI.interactableText.text = interactableObject.interactableText;
                                interactableUIGameObject.SetActive(true);

                                if (inputHandler.aInput)
                                {
                                    interactableObject.Interact(this);
                                    interactableUIGameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            BonfireInteraction interactableObject = bonManager.GetComponent<BonfireInteraction>();

                            if (interactableObject != null)
                            {
                                if (bonManager.showRestPopUp)
                                {
                                    interactableUI.interactableText.text = interactableObject.interactableText;
                                    interactableUIGameObject.SetActive(true);

                                    if (inputHandler.aInput)
                                    {
                                        interactableObject.Interact(this);
                                    }
                                }
                            }
                        }
                    }
                    else if (interactColliders[i].CompareTag(interactableTag))
                    {
                        Interactable interactableObject = interactColliders[i].GetComponent<Interactable>();

                        if (interactableObject != null)
                        {
                            interactableUI.interactableText.text = interactableObject.interactableText;
                            interactableUIGameObject.SetActive(true);

                            if (inputHandler.aInput)
                            {
                                interactableObject.Interact(this);
                            }
                        }
                    }
                    else if (interactColliders[i].CompareTag(fogWallTag))
                    {
                        FogWallManager interactableObject = interactColliders[i].GetComponent<FogWallManager>();

                        if (interactableObject != null)
                        {
                            if (interactableObject.canInteract)
                            {
                                interactableUI.interactableText.text = interactableObject.interactableText;
                                interactableUIGameObject.SetActive(true);

                                if (inputHandler.aInput)
                                {
                                    interactableObject.Interact(this);
                                }
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

                if (itemInteractableGameObject != null && inputHandler.aInput)
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
                if(healthBgRefillTimer > 1.5f)
                {
                    playerStats.healthBar.backgroundSlider.value -= playerStats.healthBgRefillAmount * delta;

                    if(playerStats.healthBar.backgroundSlider.value <= playerStats.healthBar.healthBarSlider.value)
                    {
                        playerStats.healthBar.backgroundSlider.value = playerStats.healthBar.healthBarSlider.value;
                    }
                }
                else
                {
                    healthBgRefillTimer += delta;
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
                if (staminaRefillTimer > 2.0f)
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

        private void CheckForFocusRefill(float delta)
        {
            shouldRefillFocus = !isInteracting && (playerStats.currentFocus < playerStats.maxFocus);

            if (shouldRefillFocus)
            {
                if (focusRefillTimer > 1.0f)
                {
                    playerStats.RefillFocus();
                }
                else
                {
                    focusRefillTimer += delta;
                }
            }
            else
            {
                focusRefillTimer = 0.0f;
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
                if (inputHandler.moveAmount > 0)
                {
                    playerStats.TakeStaminaDamage(playerLocomotion.sprintStaminaCost * delta);
                }

                if(playerStats.currentStamina <= 0f)
                {
                    isSprinting = false;
                }
            }
        }
        #endregion
    }
}