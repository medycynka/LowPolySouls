using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class AnimationSoundManager : MonoBehaviour
    {
        [Header("Audio Clips", order = 0)]
        [Header("Multpiple Clips For Random Pick", order = 1)]
        public AudioClip[] movingClips;
        public AudioClip[] attackingClips;
        public AudioClip[] getDamageClips;

        [Header("Unique Clips", order = 1)]
        public AudioClip rollClip;
        public AudioClip backStepClip;
        public AudioClip estusUse;
        public AudioClip soulUse;

        [Header("Current & Previouse Background Music Clip", order = 1)]
        public AudioClip currentBackgroundMusic;
        public AudioClip previouseBackgroundMusic;

        AudioSource audioSource;
        bool playFottsteps = true;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = currentBackgroundMusic;
            audioSource.volume = SettingsHolder.soundVolume;
            previouseBackgroundMusic = currentBackgroundMusic;
            audioSource.Play();
        }

        public void ChangeBackGroundMusic(AudioClip newBgMusic)
        {
            previouseBackgroundMusic = currentBackgroundMusic;
            currentBackgroundMusic = newBgMusic;
            audioSource.clip = currentBackgroundMusic;
            audioSource.Play();
        }

        public void ChangeFootstepsSound(AudioClip[] newFootSteps, AreaManager areaManager)
        {
            areaManager.SetExitFootSteps(movingClips);
            movingClips = newFootSteps;
        }

        public void ChangeFootstepsSound(AudioClip[] newFootSteps, BossAreaManager areaManager)
        {
            areaManager.SetExitFootSteps(movingClips);
            movingClips = newFootSteps;
        }

        #region Play For Animation
        public void PlayOnStep()
        {
            if (movingClips.Length > 0 && playFottsteps)
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
            playFottsteps = true;
            
        }
        
        public void DisableFootStepsSound()
        {
            playFottsteps = false;
            
        }
        
        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }

        private IEnumerator StopStepSounds()
        {
            DisableFootStepsSound();

            yield return new WaitForSeconds(1.0f);

            EnableFootStepsSound();
        }
    }
}