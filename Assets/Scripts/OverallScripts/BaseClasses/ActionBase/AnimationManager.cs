using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.BaseClasses
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
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInteractingName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName], isInteracting);
            anim.CrossFade(isEnemy ? StaticAnimatorIds.enemyAnimationIds[targetAnim] : StaticAnimatorIds.animationIds[targetAnim], 0.2f);
        }
        
        public void PlayTargetAnimation(int targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInteractingName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName], isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }

}