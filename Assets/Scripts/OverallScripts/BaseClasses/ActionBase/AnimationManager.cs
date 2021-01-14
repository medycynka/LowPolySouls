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
        public int isInteractingId;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool(isInteractingId, isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }

}