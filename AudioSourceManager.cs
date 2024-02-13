using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleAudioAndSettings
{
    public class AudioSourceManager
    {
        AudioData audioData;
        public AudioSourceManager(AudioData data)
        {
            audioData = data;
            //This will update the AudioSource volume on volume change
            if (audioData.updateVolumeOnMasterChange == true)
            {
                switch (audioData.audioType)
                {
                    case AudioType.MUSIC:
                        GlobalSettings.globalSettings.musicVolume.OnValueChange += EventUpdateVolume;
                        break;
                    case AudioType.SFX:
                        GlobalSettings.globalSettings.SFXVolume.OnValueChange += EventUpdateVolume;
                        break;
                    case AudioType.VOICE:
                        GlobalSettings.globalSettings.voiceVolume.OnValueChange += EventUpdateVolume;
                        break;
                    case AudioType.DEFAULT:
                        break;
                    default:
                        Debug.LogError("Unknown AudioType " + audioData.audioType);
                        break;
                }
                GlobalSettings.globalSettings.masterVolume.OnValueChange += EventUpdateVolume;
                UpdateVolume();
            }
        }
        ~AudioSourceManager()
        {
            if (audioData.updateVolumeOnMasterChange == true)
            {
                switch (audioData.audioType)
                {
                    case AudioType.MUSIC:
                        GlobalSettings.globalSettings.musicVolume.OnValueChange -= EventUpdateVolume;
                        break;
                    case AudioType.SFX:
                        GlobalSettings.globalSettings.SFXVolume.OnValueChange -= EventUpdateVolume;
                        break;
                    case AudioType.VOICE:
                        GlobalSettings.globalSettings.voiceVolume.OnValueChange -= EventUpdateVolume;
                        break;
                    case AudioType.DEFAULT:
                        break;
                    default:
                        Debug.LogError("Unknown AudioType " + audioData.audioType);
                        break;
                }
                GlobalSettings.globalSettings.masterVolume.OnValueChange += EventUpdateVolume;
            }
        }
        /// <summary>
        /// Plays audio clip at audioClipIndex
        /// </summary>
        /// <param name="audioClipIndex"></param>
        public void PlayAudio(int audioClipIndex)
        {
            //If replayAudioOnPlay is false and the AudioSource is playing, then it will not play
            if (CanNotPlayAgain())
            {
                return;
            }
            UpdateVolume();
            try
            {
                audioData.audioSource.clip = audioData.audioClips[audioClipIndex];
                audioData.audioSource.Stop();
                audioData.audioSource.Play();
            }
            catch
            {
                Debug.LogWarning("Invalid audio index of " + audioClipIndex + " maxIndexNum is " + (audioData.audioClips.Length - 1));
            }
        }
        /// <summary>
        /// Will play the 0th index in audioClips
        /// </summary>
        public void PlayAudio()
        {
            //If replayAudioOnPlay is false and the AudioSource is playing, then it will not play
            if (CanNotPlayAgain())
            {
                return;
            }
            PlayAudio(0);
        }
        /// <summary>
        /// Will play a random index in audioClips
        /// </summary>
        public void PlayRandom()
        {
            //If replayAudioOnPlay is false and the AudioSource is playing, then it will not play
            if (CanNotPlayAgain())
            {
                return;
            }
            PlayAudio(Random.Range(0, audioData.audioClips.Length));
        }
        /// <summary>
        /// Will play a random index in audioClips in the range of minInclusive and maxExclusive
        /// </summary>
        /// <param name="minInclusive"></param>
        /// <param name="maxExclusive"></param>
        public void PlayRandom(int minInclusive, int maxExclusive)
        {
            //If replayAudioOnPlay is false and the AudioSource is playing, then it will not play
            if (CanNotPlayAgain())
            {
                return;
            }
            PlayAudio(Random.Range(minInclusive, maxExclusive));
        }
        public void StopAudio()
        {
            audioData.audioSource.Stop();
        }
        public void SetReplayAudioOnPlay(bool replayAudioOnPlay)
        {
            audioData.replayAudioOnPlay = replayAudioOnPlay;
        }
        public float GetClipLength(int audioClipIndex)
        {
            return audioData.audioClips[audioClipIndex].length;
        }
        /// <summary>
        /// Plays an audioClip and sets the postion of the audio clip to time
        /// </summary>
        /// <param name="audioClipIndex"></param>
        /// <param name="time"></param>
        public void SetPostionAndPlay(int audioClipIndex, float time)
        {
            //If replayAudioOnPlay is false and the AudioSource is playing, then it will not play
            if (CanNotPlayAgain())
            {
                return;
            }
            PlayAudio(audioClipIndex);
            audioData.audioSource.time = time;
        }
        public float GetVolume()
        {
            return audioData.audioSource.volume;
        }
        public AudioType GetAudioType()
        {
            return audioData.audioType;
        }
        /// <summary>
        /// Checks to see if an AudioSource can play again
        /// </summary>
        /// <returns></returns>
        private bool CanNotPlayAgain()
        {
            if (audioData.replayAudioOnPlay && audioData.audioSource.isPlaying)
            {
                return true;
            }
            return false;
        }
        private void UpdateVolume()
        {
            switch (audioData.audioType)
            {
                case AudioType.MUSIC:
                    audioData.audioSource.volume = (GlobalSettings.globalSettings.musicVolume.GetValue() * GlobalSettings.globalSettings.masterVolume.GetValue());
                    break;
                case AudioType.SFX:
                    audioData.audioSource.volume = (GlobalSettings.globalSettings.SFXVolume.GetValue() * GlobalSettings.globalSettings.masterVolume.GetValue());
                    break;
                case AudioType.VOICE:
                    audioData.audioSource.volume = (GlobalSettings.globalSettings.voiceVolume.GetValue() * GlobalSettings.globalSettings.masterVolume.GetValue());
                    break;
                case AudioType.DEFAULT:
                    audioData.audioSource.volume = (GlobalSettings.globalSettings.masterVolume.GetValue());
                    break;
                default:
                    Debug.LogError("Unknown AudioType " + audioData.audioType);
                    break;
            }
        }
        private void EventUpdateVolume(object sender, System.EventArgs e)
        {
            UpdateVolume();
        }
    }

    /// <summary>
    /// Types of Audio
    /// </summary>
    public enum AudioType
    {
        DEFAULT,
        MUSIC,
        SFX,
        VOICE
    }
    /// <summary>
    /// DataStructure used to store AudioData information
    /// </summary>
    [System.Serializable]
    public struct AudioData
    {
        public AudioSource audioSource;
        public AudioClip[] audioClips;
        public AudioType audioType;
        public bool replayAudioOnPlay;//If true will replay audio even if audio is playing
        public bool updateVolumeOnMasterChange;//If true when volume in global settings is changed this volume will be updated also
    }
}
