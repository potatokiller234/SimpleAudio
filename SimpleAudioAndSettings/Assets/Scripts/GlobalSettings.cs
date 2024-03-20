using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SimpleAudioAndSettings
{
    public class GlobalSettings : MonoBehaviour
    {
        public static GlobalSettings globalSettings;
        public List<EventVariable<float>> allVolumes;
        public SimpleAudioType[] audioTypes;
        public float defaultVolumeLevel = .5f;
        //To add new AudioTypes just add a new 

        private void Awake()
        {
            globalSettings = this;
            allVolumes = new List<EventVariable<float>>();
            audioTypes =  (SimpleAudioType[])Enum.GetValues(typeof(SimpleAudioType));
            for (int i = 0; i < audioTypes.Length;i++)
            {
                allVolumes.Add(new EventVariable<float>(defaultVolumeLevel, audioTypes[i]));
            }
        }

    }
}
