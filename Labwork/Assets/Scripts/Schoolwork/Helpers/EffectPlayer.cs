using System.Collections.Generic;
using UnityEngine;

namespace Schoolwork.Helpers 
{
    public class EffectPlayer : MonoBehaviour
    {
        public List<AudioClip> soundEffects;
        [SerializeField]        
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            if (soundEffects.Count > 0)
            {
                int randomIndex = Random.Range(0, soundEffects.Count);
                audioSource.clip = soundEffects[randomIndex];
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("No sound effects assigned to EffectPlayer.");
            }
        }
    }
}

