using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon;

        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot backSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        Animator animator;

        public QuickSlotsUI quickSlotsUI;

        PlayerStats playerStats;
        InputHandler inputHandler;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
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
                    animator.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
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
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(isLeft, weaponItem);
                }
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.TH_Idle, 0.2f);
                }
                else
                {
                    #region Handle Right Weapon Idle Animation

                    animator.CrossFade("Both Arms Empty", 0.2f);
                    backSlot.UnloadWeaponAndDestroy();

                    if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
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

        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisaleDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisaleDamageCollider();
        }

        #endregion

        #region Handle Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.oh_lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.oh_heavyAttackMultiplier));
        }

        public void DrainStaminaLightAttackTH()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.th_lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttackTH()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.th_heavyAttackMultiplier));
        }

        #endregion
    }

}
