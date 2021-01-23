using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class AnimationSoundManager : MonoBehaviour
    {
        [Header("Audio Clips", order = 0)]
        [Header("Multiple Clips For Random Pick", order = 1)]
        public AudioClip[] movingClips;
        public AudioClip[] attackingClips;
        public AudioClip[] getDamageClips;
        public AudioClip[] faithSkillClips;
        public AudioClip[] curseSkillClips;
        public AudioClip[] destructionSkillClips;

        [Header("Unique Clips", order = 1)]
        public AudioClip rollClip;
        public AudioClip backStepClip;
        public AudioClip estusUse;
        public AudioClip soulUse;

        [Header("Current Background Music Clip", order = 1)]
        public AudioClip currentBackgroundMusic;
        //public AudioClip previouseBackgroundMusic;

        [Header("Music Fade In/Out")]
        public float musicFadeIn = 2f;
        public float musicFadeOut = 2f;
        public bool fadingMusic = false;
        
        AudioSource audioSource;
        bool playFootsteps = true;
        private float currTime = 0.0f;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = currentBackgroundMusic;
            audioSource.volume = SettingsHolder.soundVolume;
            //previouseBackgroundMusic = currentBackgroundMusic;
            audioSource.Play();
        }

        public void ChangeBackGroundMusic(AudioClip newBgMusic)
        {
            currentBackgroundMusic = newBgMusic;
            audioSource.clip = currentBackgroundMusic;
            audioSource.volume = SettingsHolder.soundVolume;
            audioSource.Play();
        }

        public void ChangeFootstepsSound(AudioClip[] newFootSteps, AreaManager areaManager)
        {
            areaManager.SetExitFootSteps(movingClips);
            
            if (newFootSteps.Length > 0)
            {
                movingClips = newFootSteps;
            }
        }

        public void ChangeFootstepsSound(AudioClip[] newFootSteps, BossAreaManager areaManager)
        {
            areaManager.SetExitFootSteps(movingClips);
            
            if (newFootSteps.Length > 0)
            {
                movingClips = newFootSteps;
            }
        }

        #region Play For Animation
        public void PlayOnStep()
        {
            if (movingClips.Length > 0 && playFootsteps)
            {
                audioSource.PlayOneShot(GetRandomClip(movingClips));
            }
        }

        public void PlayOnRoll()
        {
            if (rollClip != null)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(rollClip);
            }
        }

        public void PlayOnBackStep()
        {
            if (backStepClip != null)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(backStepClip);
            }
        }

        public void PlayOnAttack()
        {
            if (movingClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(GetRandomClip(attackingClips));
            }
        }
        
        public void PlayOnFaith()
        {
            if (faithSkillClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(GetRandomClip(faithSkillClips));
            }
        }
        
        public void PlayOnCurse()
        {
            if (curseSkillClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(GetRandomClip(curseSkillClips));
            }
        }
        
        public void PlayOnDestruction()
        {
            if (destructionSkillClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(GetRandomClip(destructionSkillClips));
            }
        }

        public void PlayOnDamage()
        {
            if (getDamageClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(GetRandomClip(getDamageClips));
            }
        }

        public void PlayOnEstusUse()
        {
            if (estusUse != null)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(estusUse);
            }
        }

        public void PlayOnSoulUse()
        {
            if (soulUse != null)
            {
                StartCoroutine(StopStepSounds());
                audioSource.PlayOneShot(soulUse);
            }
        }
        #endregion

        public void EnableFootStepsSound()
        {
            playFootsteps = true;
            
        }
        
        public void DisableFootStepsSound()
        {
            playFootsteps = false;
            
        }
        
        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }

        private IEnumerator StopStepSounds()
        {
            DisableFootStepsSound();

            yield return CoroutineYielder.stepSoundStopWaiter;
            
            EnableFootStepsSound();
        }
    }
}