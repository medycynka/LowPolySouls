using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class AnimationSoundManager : MonoBehaviour
    {
        [Header("Audio Clips")]
        public AudioClip[] movingClips;
        public AudioClip rollClips;
        public AudioClip[] attackingClips;
        public AudioClip[] getDamageClips;
        public AudioClip estusUse;
        public AudioClip soulUse;

        [Header("Current Background Music Clip")]
        public AudioClip currentBackgroundMusic;
        public AudioClip previouseBackgroundMusic;

        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = currentBackgroundMusic;
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

        #region Play For Animation
        public void PlayOnStep()
        {
            audioSource.PlayOneShot(GetRandomClip(movingClips), 2.0f);
        }

        public void PlayOnRoll()
        {
            audioSource.PlayOneShot(rollClips);
        }

        public void PlayOnAttack()
        {
            audioSource.PlayOneShot(GetRandomClip(movingClips));
        }

        public void PlayOnDamage()
        {
            audioSource.PlayOneShot(GetRandomClip(movingClips));
        }

        public void PlayOnEstusUse()
        {
            audioSource.PlayOneShot(estusUse);
        }

        public void PlayOnSoulUse()
        {
            audioSource.PlayOneShot(soulUse);
        }
        #endregion

        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }
    }
}