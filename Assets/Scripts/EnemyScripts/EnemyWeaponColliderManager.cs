using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class EnemyWeaponColliderManager : MonoBehaviour
    {
        public DamageCollider damageCollider;

        private void Awake()
        {
            damageCollider = GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            damageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            damageCollider.DisaleDamageCollider();
        }
    }
}