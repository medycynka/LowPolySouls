using SzymonPeszek.Items.Weapons;
using UnityEngine;


namespace SzymonPeszek.Misc.ColliderManagers
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingCollider;
        public float blockingPhysicalDamageAbsorption;

        private void Awake()
        {
            blockingCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsorption(WeaponItem weapon)
        {
            if (weapon != null)
            {
                blockingPhysicalDamageAbsorption = weapon.physicalDamageAbsorption;
            }
        }

        public void EnableBlockingCollider()
        {
            blockingCollider.enabled = true;
        }

        public void DisableBlockingCollider()
        {
            blockingCollider.enabled = false;
        }
    }
}
