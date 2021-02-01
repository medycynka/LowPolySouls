using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.BaseClasses
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Manager", order = 0)]
        [Header("Lock-on", order = 1)]
        public Transform lockOnTransform;
        
        [Header("Combat Colliders", order = 1)]
        public BoxCollider backStabBoxCollider;
        public BackStabCollider backStabCollider;
    }
}