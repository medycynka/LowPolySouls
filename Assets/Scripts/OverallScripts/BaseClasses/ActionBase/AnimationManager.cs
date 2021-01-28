using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class AnimationManager : MonoBehaviour
    {
        [Header("Animation Manager", order = 0)]
        [Header("Animator", order = 1)]
        public Animator anim;
        public bool isEnemy;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool(isEnemy ? StaticAnimatorIds.EnemyAnimationIds[StaticAnimatorIds.IsInteractingName] : StaticAnimatorIds.AnimationIds[StaticAnimatorIds.IsInteractingName], isInteracting);
            anim.CrossFade(isEnemy ? StaticAnimatorIds.EnemyAnimationIds[targetAnim] : StaticAnimatorIds.AnimationIds[targetAnim], 0.2f);
        }
        
        public void PlayTargetAnimation(int targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool(isEnemy ? StaticAnimatorIds.EnemyAnimationIds[StaticAnimatorIds.IsInteractingName] : StaticAnimatorIds.AnimationIds[StaticAnimatorIds.IsInteractingName], isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }

}