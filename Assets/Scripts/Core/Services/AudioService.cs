using System.Collections.Generic;
using Core.Enums;
using NM.Core.Interface;
using UnityEngine;

namespace NM.Core.Services
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private List<AudioClip> clips;
        [SerializeField] private AudioSource soundSource;

        private Dictionary<AudioKey, AudioClip> audioClipsDictionary = new Dictionary<AudioKey, AudioClip>();

        private void Awake()
        {
            for (int i = 0; i < clips.Count; i++)
            {
                audioClipsDictionary.Add((AudioKey) i, clips[i]);
            }
        }

        public void PlayOneShotAudioSound(AudioKey audioKey)
        {
            var isExist = audioClipsDictionary.TryGetValue(audioKey, out var audioClip);
            if (!isExist) return;
            soundSource.PlayOneShot(audioClip);
        }

        public void PlayOneShotAudioSound(AudioSource audioSource, AudioKey audioKey)
        {
            var isExist = audioClipsDictionary.TryGetValue(audioKey, out var audioClip);
            if (!isExist) return;
            audioSource.PlayOneShot(audioClip);
        }
    }
}