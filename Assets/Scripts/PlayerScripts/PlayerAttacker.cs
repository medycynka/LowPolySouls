using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerInventory playerInventory;
        PlayerManager playerManager;

        [Header("Last Attack Name")]
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponent<AnimatorHandler>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerManager = GetComponentInParent<PlayerManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool(animatorHandler.canDoComboId, false);

                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                    lastAttack = weapon.OH_Light_Attack_2;
                }
                else if (lastAttack == weapon.OH_Light_Attack_2)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_3, true);
                } 
                else if(lastAttack == weapon.OH_Heavy_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_2, true);
                }
                else if (lastAttack == weapon.TH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                    lastAttack = weapon.TH_Light_Attack_2;
                }
                else if (lastAttack == weapon.TH_Light_Attack_2)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_3, true);
                }
                else if (lastAttack == weapon.TH_Heavy_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_2, true);
                    lastAttack = weapon.TH_Heavy_Attack_2;
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
                lastAttack = weapon.TH_Light_Attack_1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_1, true);
                lastAttack = weapon.TH_Heavy_Attack_1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                lastAttack = weapon.OH_Heavy_Attack_1;
            }
        }

        public void HandleRBAction()
        {
            switch (playerInventory.rightWeapon.weaponType)
            {
                case WeaponType.Melee:
                    PerformRBMeleeAction();
                    break;
                case WeaponType.Casting:
                    PerformRBMagicAction(playerInventory.rightWeapon);
                    break;
                case WeaponType.Shooting:
                    break;
                default:
                    break;
            }
        }
        
        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
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

                animatorHandler.anim.SetBool(animatorHandler.usingRightId, true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (weapon.castingType == CastingType.Faith)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.spellType == CastingType.Faith)
                {
                    //CHECK FOR FP
                    //ATTEMPT TO CAST SPELL
                }
            }
        }
        #endregion
    }

}