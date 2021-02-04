using UnityEngine;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.Damage;
using SzymonPeszek.GameUI.Slots;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts.Animations;


namespace SzymonPeszek.Items.Weapons
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private PlayerManager _playerManager;

        [Header("Weapon Slot Manager", order = 0)]
        [Header("Current Weapon", order = 1)]
        public WeaponItem attackingWeapon;

        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;
        private WeaponHolderSlot _backSlot;

        [Header("Left and Right Damage Colliders", order = 1)]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        private PlayerAnimatorHandler _playerAnimatorHandler;

        [Header("Quick Slots", order = 1)]
        public QuickSlotsUI quickSlotsUI;

        private PlayerStats _playerStats;
        private InputHandler _inputHandler;
        private WeaponHolderSlot[] _weaponHolderSlots;

        private void Awake()
        {
            _playerManager = GetComponentInParent<PlayerManager>();
            _playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
            _playerStats = GetComponentInParent<PlayerStats>();
            _inputHandler = GetComponentInParent<InputHandler>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

            _weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in _weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    _leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    _rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot) {
                    _backSlot = weaponSlot;
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
                    _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[weaponItem.leftHandIdle], false);
                }
                else
                {
                    _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.LeftArmEmptyName], false);
                }
                #endregion

                if (!_inputHandler.twoHandFlag)
                {
                    _leftHandSlot.currentWeapon = weaponItem;
                    _leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                }
                else
                {
                    _backSlot.currentWeapon = weaponItem;
                    _backSlot.LoadWeaponModel(weaponItem);
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                }
            }
            else
            {
                if (_inputHandler.twoHandFlag)
                {
                    _backSlot.LoadWeaponModel(_leftHandSlot.currentWeapon);
                    _leftHandSlot.UnloadWeaponAndDestroy();
                    _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[weaponItem.thIdle], false);
                }
                else
                {
                    #region Handle Right Weapon Idle Animation

                    _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.BothArmsEmptyName], false);
                    _backSlot.UnloadWeaponAndDestroy();

                    if (weaponItem != null)
                    {
                        _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[weaponItem.rightHandIdle], false);
                    }
                    else
                    {
                        _playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.RightArmEmptyName], false);
                    }
                    #endregion
                }

                _rightHandSlot.currentWeapon = weaponItem;
                _rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = _leftHandSlot.currentWeapon.baseAttack;
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = _rightHandSlot.currentWeapon.baseAttack;
        }

        public void OpenDamageCollider()
        {
            if (_playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (_playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamageCollider.DisableDamageCollider();
        }

        #endregion

        #region Handle Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.ohLightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.ohHeavyAttackMultiplier));
        }

        public void DrainStaminaLightAttackTH()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.thLightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttackTH()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.thHeavyAttackMultiplier));
        }

        #endregion
    }

}
