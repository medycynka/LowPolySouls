using UnityEngine;


namespace SzymonPeszek.Items.Weapons
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        [Header("Weapon Holder", order = 0)]
        [Header("Properties", order = 1)]
        public Transform parentOverride;
        public WeaponItem currentWeapon;
        public GameObject currentWeaponModel;

        [Header("Bools", order = 1)]
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;

        private void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab);
            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;

                    if (isLeftHandSlot)
                    {
                        model.transform.GetChild(0).transform.localPosition = weaponItem.pivotPositionTransform;
                        model.transform.GetChild(0).transform.localEulerAngles = weaponItem.pivotRotationTransform;
                    }
                }
                else
                {
                    model.transform.parent = transform;
                }

                if (isBackSlot)
                {
                    model.transform.localPosition = weaponItem.backSlotPosition;
                    model.transform.localEulerAngles = weaponItem.backSlotRotation;
                    model.transform.localScale = Vector3.one * weaponItem.backSlotScale;
                }
                else
                {
                    model.transform.localPosition = Vector3.zero;
                    model.transform.localRotation = Quaternion.identity;
                    model.transform.localScale = Vector3.one;
                }
            }

            currentWeaponModel = model;
        }
    }

}