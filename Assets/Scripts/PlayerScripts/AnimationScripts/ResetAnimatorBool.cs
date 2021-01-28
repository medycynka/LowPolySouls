using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        [Header("Animator's Bool Reset", order = 0)] 
        [Header("Properties", order = 1)]
        public bool isEnemy;
        public string[] targetBools;
        public bool[] status;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isEnemy)
            {
                for (int i = 0; i < targetBools.Length; i++)
                {
                    animator.SetBool(StaticAnimatorIds.EnemyAnimationIds[targetBools[i]], status[i]);
                }
            }
            else
            {
                for (int i = 0; i < targetBools.Length; i++)
                {
                    animator.SetBool(StaticAnimatorIds.AnimationIds[targetBools[i]], status[i]);
                }
            }

            //animator.SetBool(targetBool, status);
        }
    }
}