using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
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