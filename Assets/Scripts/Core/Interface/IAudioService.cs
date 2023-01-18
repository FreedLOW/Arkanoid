using Core.Enums;
using UnityEngine;

namespace NM.Core.Interface
{
    public interface IAudioService
    {
        public void PlayOneShotAudioSound(AudioKey audioKey);
        public void PlayOneShotAudioSound(AudioSource audioSource, AudioKey audioKey);
    }
}