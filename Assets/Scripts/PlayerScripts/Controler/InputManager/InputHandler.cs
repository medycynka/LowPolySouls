using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Handler",order = 0)]
        [Header("Movement",order = 1)]
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        [Header("Inputs", order = 1)]
        public bool b_Input;
        public bool a_Input;
        public bool y_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool critical_Attack_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;
        public bool estusQuickSlotUse;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        [Header("Flags", order = 1)]
        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        
        [Header("Back Stabs", order = 1)]
        public Transform criticalAttackRayCastStartPoint;

        [Header("Roll Timer", order = 1)]
        public float rollInputTimer;

        private PlayerControls playerInputActions;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;
        private PlayerStats playerStats;
        private WeaponSlotManager weaponSlotManager;
        private AnimatorHandler animatorHandler;

        [Header("Camera & UI", order = 1)]
        public CameraHandler cameraHandler;
        public UIManager uiManager;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
        }

        public void OnEnable()
        {
            if (playerInputActions == null)
            {
                playerInputActions = new PlayerControls();
                playerInputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                playerInputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerInputActions.PlayerActions.RB.performed += i => rb_Input = true;
                playerInputActions.PlayerActions.RT.performed += i => rt_Input = true;
                playerInputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                playerInputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                playerInputActions.PlayerActions.E.performed += i => a_Input = true;
                playerInputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                playerInputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                playerInputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                playerInputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                playerInputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                playerInputActions.PlayerActions.Y.performed += i => y_Input = true;
                playerInputActions.PlayerActions.EstusQuickSlotUse.performed += i => estusQuickSlotUse = true;
                playerInputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;
            }

            playerInputActions.Enable();
        }

        private void OnDisable()
        {
            playerInputActions.Disable();
        }

        public void TickInput(float delta)
        {
            if (playerStats.isPlayerAlive && !playerManager.isRestingAtBonfire && !playerManager.isRemovingFog)
            {
                if (!inventoryFlag)
                {
                    HandleMoveInput(delta);
                    HandleRollInput(delta);
                    HandleAttackInput(delta);
                    HandleQuickSlotsInput();
                    HandleLockOnInput();
                    HandleTwoHandInput();
                    HandleQuickHealInput();
                    HandleCriticalAttackInput();
                }

                HandleInventoryInput();
            }
        }

        private void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_Input = playerInputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            sprintFlag = b_Input;

            if (b_Input)
            {
                rollInputTimer += delta;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            if (playerStats.currentStamina > 0)
            {
                #region Handle Light Attack
                if (rb_Input)
                {
                    playerAttacker.HandleRbAction();
                }
                #endregion

                #region Handle Heavy Attack
                if (rt_Input)
                {
                    if (playerManager.canDoCombo)
                    {
                        comboFlag = true;
                        playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                        comboFlag = false;
                    }
                    else
                    {
                        if (playerManager.isInteracting)
                        {
                            return;
                        }

                        if (playerManager.canDoCombo)
                        {
                            return;
                        }

                        animatorHandler.anim.SetBool(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.IsUsingRightHandName], true);
                        playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
                    }
                }
                #endregion
            }
        }

        private void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                uiManager.UpdateUI();
                uiManager.ResetTabsSelection();
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    horizontal = 0f;
                    vertical = 0f;
                    moveAmount = 0f;
                    mouseX = cameraInput.x;
                    mouseY = cameraInput.y;

                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.ResetTabsSelection();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput)
            {
                if (lockOnFlag)
                {
                    lockOnInput = false;
                    lockOnFlag = false;
                    cameraHandler.ClearLockOnTargets();
                }
                else
                {
                    lockOnInput = false;
                    cameraHandler.HandleLockOn();

                    if (cameraHandler.nearestLockOnTarget != null)
                    {
                        cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                        lockOnFlag = true;
                    }
                }
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;

                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }

        private void HandleQuickHealInput()
        {
            if (estusQuickSlotUse && uiManager.GetEstusCountInInventory() > 0)
            {
                ConsumableItem cI = playerInventory.consumablesInventory.Find(e => e.consumableType == ConsumableType.HealItem);
                playerStats.healthRefillAmount = cI.healAmount;
                playerInventory.consumablesInventory.Remove(cI);
                playerManager.shouldRefillHealth = true;
                animatorHandler.PlayTargetAnimation(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.EstusName], true);
                uiManager.UpdateEstusAmount();
            }
        }
        
        private void HandleCriticalAttackInput()
        {
            if (critical_Attack_Input)
            {
                critical_Attack_Input = false;
                playerAttacker.AttemptBackStabOrRiposte();
            }
        }
    }
}
