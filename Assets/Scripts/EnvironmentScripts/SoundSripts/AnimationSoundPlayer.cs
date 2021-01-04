using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP {
    public class AnimationSoundPlayer : MonoBehaviour
    {
        AnimationSoundManager animationSoundManager;

        public bool canPlayStep = true;

        private void Awake()
        {
            animationSoundManager = GetComponentInParent<AnimationSoundManager>();
        }

        public void StepSound()
        {
            if (canPlayStep)
            {
                animationSoundManager.PlayOnStep();
            }
        }

        public void RollSound()
        {
            animationSoundManager.PlayOnRoll();
        }

        public void AttackSound()
        {
            animationSoundManager.PlayOnAttack();
        }

        public void DamageSound()
        {
            animationSoundManager.PlayOnDamage();
        }

        public void EstusSound()
        {
            animationSoundManager.PlayOnEstusUse();
        }

        public void SoulSound()
        {
            animationSoundManager.PlayOnSoulUse();
        }
    }
}