using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleAudioAndSettings
{
    public class AudioSourceManager
    {
        AudioData audioData;
        readonly EventVariable<float> masterVolume;
        public AudioSourceManager(AudioData data)
        {
            audioData = data;

            //Get masterVolume ref
            for (int i = 0; i < GlobalSettings.globalSettings.allVolumes.Count; i++)
            {
                if (SimpleAudioType.MASTER == GlobalSettings.globalSettings.allVolumes[i].type)
                {
                    masterVolume = GlobalSettings.globalSettings.allVolumes[i];
                    break;
                }
            }

            //Return if volume should not change onMasterVolimeChange
            if (audioData.updateVolumeOnMasterChange == false)
                return;
            //If this audio is a master then update on master change
            if (SimpleAudioType.MASTER == audioData.audioType)
            {
                masterVolume.OnValueChange += EventUpdateVolume;
            }
            else
            {
                //Add the event listeners to update volume OnValueChange
                for (int i = 0; i < GlobalSettings.globalSettings.allVolumes.Count; i++)
                {
                    if (audioData.audioType == GlobalSettings.globalSettings.allVolumes[i].type)
                    {
                        GlobalSettings.globalSettings.allVolumes[i].OnValueChange += EventUpdateVolume;
                        break;
                    }
                }
                masterVolume.OnValueChange += EventUpdateVolume;
            }
            //Updates the volume off rip
            UpdateVolume();
            return;

        }
        //Removes all EventUpdateVolume
        ~AudioSourceManager()
        {
            if (audioData.updateVolumeOnMasterChange == true)
            {
                if (SimpleAudioType.MASTER == audioData.audioType)
                {
                    masterVolume.OnValueChange -= EventUpdateVolume;
                    return;
                }
                else
                {
                    //Add the event listeners to update volume OnValueChange
                    for (int i = 0; i < GlobalSettings.globalSettings.allVolumes.Count; i++)
                    {
                        if (audioData.audioType == GlobalSettings.globalSettings.allVolumes[i].type)
                        {
                            GlobalSettings.globalSettings.allVolumes[i].OnValueChange -= EventUpdateVolume;
                            break;
                        }
                    }
                    masterVolume.OnValueChange -= EventUpdateVolume;
                }
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
        public SimpleAudioType GetAudioType()
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
            //If master update based on master volume
            if (audioData.audioType == SimpleAudioType.MASTER)
            {
                audioData.audioSource.volume = masterVolume.GetValue();
                return;
            }
            //Else try the other volume types
            for (int i = 0; i < GlobalSettings.globalSettings.allVolumes.Count; i++)
            { 
                if (audioData.audioType == GlobalSettings.globalSettings.allVolumes[i].type)
                {
                    audioData.audioSource.volume = GlobalSettings.globalSettings.allVolumes[i].GetValue() * masterVolume.GetValue();
                    return;
                }
            }
        }
        private void EventUpdateVolume(object sender, System.EventArgs e)
        {
            UpdateVolume();
        }
    }

    /// <summary>
    /// DataStructure used to store AudioData information
    /// </summary>
    [System.Serializable]
    public struct AudioData
    {
        public AudioSource audioSource;
        public AudioClip[] audioClips;
        public SimpleAudioType audioType;
        public bool replayAudioOnPlay;//If true will replay audio even if audio is playing
        public bool updateVolumeOnMasterChange;//If true when volume in global settings is changed this volume will be updated also
    }
}
