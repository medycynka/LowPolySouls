using System;
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
        PlayerStats playerStats;
        
        RaycastHit hit;
        public LayerMask backStabLayer;

        [Header("Last Attack Name")]
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponent<AnimatorHandler>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            backStabLayer = 1 << LayerMask.NameToLayer("Back Stab");
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.CanDoComboName], false);

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

        public void HandleRbAction()
        {
            switch (playerInventory.rightWeapon.weaponType)
            {
                case WeaponType.Melee:
                    PerformRbMeleeAction();
                    break;
                case WeaponType.Casting:
                    PerformRbMagicAction(playerInventory.rightWeapon);
                    break;
                case WeaponType.Shooting:
                    break;
                default:
                    break;
            }
        }
        
        #region Attack Actions
        private void PerformRbMeleeAction()
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

                animatorHandler.anim.SetBool(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.IsUsingRightHandName], true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        private void PerformRbMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            
            if (playerInventory.currentSpell != null && playerStats.currentFocus > 0)
            {
                if (weapon.castingType == CastingType.Faith)
                {
                    if (playerInventory.currentSpell.spellType == CastingType.Faith)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
                    }
                }
            }
        }
        
        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
        }
        
        public void AttemptBackStabOrRiposte()
        {
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, 
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                EnemyManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<EnemyManager>();

                if (enemyCharacterManager != null)
                {
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.backStabberStandPoint.position;
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;
                    
                    animatorHandler.PlayTargetAnimation(StaticAnimatorIds.AnimationIds[StaticAnimatorIds.BackStabName], true);
                    enemyCharacterManager.HandleGettingBackStabbed(playerInventory.rightWeapon.backStabDamage);
                }
            }
        }
        #endregion
    }

}