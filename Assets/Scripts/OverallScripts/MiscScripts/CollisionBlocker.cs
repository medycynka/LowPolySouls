using UnityEngine;

namespace SzymonPeszek.Misc
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