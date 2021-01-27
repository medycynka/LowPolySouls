using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class CollisionBlocker : MonoBehaviour
    {
        public CapsuleCollider characterCollider;
        public CapsuleCollider blockerCollider;

        void Start()
        {
            Physics.IgnoreCollision(characterCollider, blockerCollider, true);
        }
    }
}