using UnityEngine;
using SzymonPeszek.Damage;


namespace SzymonPeszek.EnemyScripts
{
    public class EnemyWeaponColliderManager : MonoBehaviour
    {
        [Header("Weapon Collider Manager", order = 0)]
        [Header("Collider", order = 1)]
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
            damageCollider.DisableDamageCollider();
        }
    }
}