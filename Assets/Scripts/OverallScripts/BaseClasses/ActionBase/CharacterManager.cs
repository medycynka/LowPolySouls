using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for character (player or enemy) manager
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        [Header("Manager", order = 0)]
        [Header("Lock-on", order = 1)]
        public Transform lockOnTransform;
        
        [Header("Combat Colliders", order = 1)]
        public BoxCollider backStabBoxCollider;
        public BackStabCollider backStabCollider;

        [Header("Combat Colliders", order = 1)]
        public Transform characterTransform;
        
        [Header("Critical Damage for Animations", order = 1)]
        public float pendingCriticalDamage;
    }
}