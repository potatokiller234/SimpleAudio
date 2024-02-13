using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleAudioAndSettings
{
    public class GlobalSettings : MonoBehaviour
    {
        public static GlobalSettings globalSettings;
        public EventVariable<float> masterVolume;
        public EventVariable<float> SFXVolume;
        public EventVariable<float> musicVolume;
        public EventVariable<float> voiceVolume;
        //To add a new Volume type such as background noise first create a new public EventVariable, Initialize it in the Awake function
        //Add a new Enum into AudioType in AudioSourceManager
        //Go to AudioSourceManager and add a new case to AudioSourceManager constructer, destructor, and VolumeUpdate()

        private void Awake()
        {
            globalSettings = this;
            masterVolume = new EventVariable<float>(.5f);
            SFXVolume = new EventVariable<float>(.5f);
            musicVolume = new EventVariable<float>(.5f);
            voiceVolume = new EventVariable<float>(.5f);
        }

    }
}
