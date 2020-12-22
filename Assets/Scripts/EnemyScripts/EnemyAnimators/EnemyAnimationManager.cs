using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class EnemyAnimationManager : AnimationManager
    {
        EnemyLocomotionManager enemyLocomotionManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyLocomotionManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            
            if (delta != 0.0f)
            {
                Vector3 velocity = deltaPosition / delta;
                enemyLocomotionManager.enemyRigidBody.velocity = velocity;
            }
            else
            {
                enemyLocomotionManager.enemyRigidBody.velocity = Vector3.zero;
            }
        }
    }

}
