using UnityEngine;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.Damage;
using SzymonPeszek.GameUI.Slots;


namespace SzymonPeszek.Items.Weapons
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private PlayerManager playerManager;

        [Header("Weapon Slot Manager", order = 0)]
        [Header("Current Weapon", order = 1)]
        public WeaponItem attackingWeapon;

        private WeaponHolderSlot leftHandSlot;
        private WeaponHolderSlot rightHandSlot;
        private WeaponHolderSlot backSlot;

        private DamageCollider leftHandDamageCollider;
        private DamageCollider rightHandDamageCollider;

        private Animator animator;

        [Header("Quick Slots", order = 1)]
        public QuickSlotsUI quickSlotsUI;

        private PlayerStats playerStats;
        private InputHandler inputHandler;
        private WeaponHolderSlot[] weaponHolderSlots;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

            weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot) {
                    backSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                #region Handle Left Weapon Idle Animation
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.leftHandIdle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion

                if (!inputHandler.twoHandFlag)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(isLeft, weaponItem);
                }
                else
                {
                    backSlot.currentWeapon = weaponItem;
                    backSlot.LoadWeaponModel(weaponItem);
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(isLeft, weaponItem);
                }
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.thIdle, 0.2f);
                }
                else
                {
                    #region Handle Right Weapon Idle Animation

                    animator.CrossFade("Both Arms Empty", 0.2f);
                    backSlot.UnloadWeaponAndDestroy();

                    if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.rightHandIdle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right Arm Empty", 0.2f);
                    }
                    #endregion
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(isLeft, weaponItem);
            }
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisaleDamageCollider();
            leftHandDamageCollider.DisaleDamageCollider();
        }

        #endregion

        #region Handle Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.ohLightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.ohHeavyAttackMultiplier));
        }

        public void DrainStaminaLightAttackTH()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.thLightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttackTH()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.thHeavyAttackMultiplier));
        }

        #endregion
    }

}
