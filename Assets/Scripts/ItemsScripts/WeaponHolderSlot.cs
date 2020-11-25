using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public WeaponItem currentWeapon;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon()
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

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
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
                }
                else
                {
                    model.transform.localPosition = Vector3.zero;
                }
                
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }

}