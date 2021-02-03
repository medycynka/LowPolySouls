using UnityEngine;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Misc;
using SzymonPeszek.Enums;
using SzymonPeszek.EnemyScripts;


namespace SzymonPeszek.PlayerScripts
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler _animatorHandler;
        private InputHandler _inputHandler;
        private WeaponSlotManager _weaponSlotManager;
        private PlayerInventory _playerInventory;
        private PlayerManager _playerManager;
        private PlayerStats _playerStats;
        private RaycastHit _hit;
        
        public LayerMask backStabLayer;

        [Header("Last Attack Name")]
        public string lastAttack;

        private void Awake()
        {
            _animatorHandler = GetComponent<AnimatorHandler>();
            _weaponSlotManager = GetComponent<WeaponSlotManager>();
            _inputHandler = GetComponentInParent<InputHandler>();
            _playerInventory = GetComponentInParent<PlayerInventory>();
            _playerManager = GetComponentInParent<PlayerManager>();
            _playerStats = GetComponentInParent<PlayerStats>();
            backStabLayer = 1 << LayerMask.NameToLayer("Back Stab");
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (_inputHandler.comboFlag)
            {
                _animatorHandler.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanDoComboName], false);

                if (lastAttack == weapon.ohLightAttack1)
                {
                    _animatorHandler.PlayTargetAnimation(weapon.ohLightAttack2, true);
                    lastAttack = weapon.ohLightAttack2;
                }
                else if (lastAttack == weapon.ohLightAttack2)
                {
                    _animatorHandler.PlayTargetAnimation(weapon.ohLightAttack3, true);
                } 
                else if(lastAttack == weapon.ohHeavyAttack1)
                {
                    _animatorHandler.PlayTargetAnimation(weapon.ohHeavyAttack2, true);
                }
                else if (lastAttack == weapon.thLightAttack1)
                {
                    _animatorHandler.PlayTargetAnimation(weapon.thLightAttack2, true);
                    lastAttack = weapon.thLightAttack2;
                }
                else if (lastAttack == weapon.thLightAttack2)
                {
                    _animatorHandler.PlayTargetAnimation(weapon.thLightAttack3, true);
                }
                else if (lastAttack == weapon.thHeavyAttack1)
                {
                    _animatorHandler.PlayTargetAnimation(weapon.thHeavyAttack2, true);
                    lastAttack = weapon.thHeavyAttack2;
                }
            }
        }

        private void HandleLightAttack(WeaponItem weapon)
        {
            _weaponSlotManager.attackingWeapon = weapon;

            if (_inputHandler.twoHandFlag)
            {
                _animatorHandler.PlayTargetAnimation(weapon.thLightAttack1, true);
                lastAttack = weapon.thLightAttack1;
            }
            else
            {
                _animatorHandler.PlayTargetAnimation(weapon.ohLightAttack1, true);
                lastAttack = weapon.ohLightAttack1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            _weaponSlotManager.attackingWeapon = weapon;

            if (_inputHandler.twoHandFlag)
            {
                _animatorHandler.PlayTargetAnimation(weapon.thHeavyAttack1, true);
                lastAttack = weapon.thHeavyAttack1;
            }
            else
            {
                _animatorHandler.PlayTargetAnimation(weapon.ohHeavyAttack1, true);
                lastAttack = weapon.ohHeavyAttack1;
            }
        }

        public void HandleRbAction()
        {
            switch (_playerInventory.rightWeapon.weaponType)
            {
                case WeaponType.Melee:
                    PerformRbMeleeAction();
                    break;
                case WeaponType.Casting:
                    PerformRbMagicAction(_playerInventory.rightWeapon);
                    break;
                case WeaponType.Shooting:
                    break;
            }
        }
        
        #region Attack Actions
        private void PerformRbMeleeAction()
        {
            if (_playerManager.canDoCombo)
            {
                _inputHandler.comboFlag = true;
                HandleWeaponCombo(_playerInventory.rightWeapon);
                _inputHandler.comboFlag = false;
            }
            else
            {
                if (_playerManager.isInteracting)
                {
                    return;
                }

                if (_playerManager.canDoCombo)
                {
                    return;
                }

                _animatorHandler.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsUsingRightHandName], true);
                HandleLightAttack(_playerInventory.rightWeapon);
            }
        }

        private void PerformRbMagicAction(WeaponItem weapon)
        {
            if (_playerManager.isInteracting)
            {
                return;
            }
            
            if (_playerInventory.currentSpell != null && _playerStats.currentFocus > 0)
            {
                if (weapon.castingType == CastingType.Faith)
                {
                    if (_playerInventory.currentSpell.spellType == CastingType.Faith)
                    {
                        _playerInventory.currentSpell.AttemptToCastSpell(_animatorHandler, _playerStats);
                    }
                }
            }
        }
        
        private void SuccessfullyCastSpell()
        {
            _playerInventory.currentSpell.SuccessfullyCastSpell(_animatorHandler, _playerStats);
        }
        
        public void AttemptBackStabOrRiposte()
        {
            if (Physics.Raycast(_inputHandler.criticalAttackRayCastStartPoint.position, 
                transform.TransformDirection(Vector3.forward), out _hit, 0.5f, backStabLayer))
            {
                EnemyManager enemyCharacterManager = _hit.transform.gameObject.GetComponentInParent<EnemyManager>();

                if (enemyCharacterManager != null)
                {
                    _playerManager.transform.position = enemyCharacterManager.backStabCollider.backStabberStandPoint.position;
                    Vector3 rotationDirection = _hit.transform.position - _playerStats.characterTransform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(_playerStats.characterTransform.rotation, tr, 500 * Time.deltaTime);
                    _playerStats.characterTransform.rotation = targetRotation;
                    
                    _animatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.BackStabName], true);
                    enemyCharacterManager.HandleGettingBackStabbed(_playerInventory.rightWeapon.backStabDamage);
                }
            }
        }
        #endregion
    }

}