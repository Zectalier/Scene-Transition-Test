using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Transitions
{
    public class TransitionSoundEvents : MonoBehaviour
    {
        public AudioClip transitionSound;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component is missing on TransitionSoundEvents.");
            }
        }

        /// <summary>
        /// Function to be called in the animation event to play the transition sound.
        /// </summary>
        public void PlayTransitionSound()
        {
            if (transitionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(transitionSound);
            }
            else
            {
                Debug.LogWarning("Transition sound or AudioSource is not set.");
            }
        }
    }
}
