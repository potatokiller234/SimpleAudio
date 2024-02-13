using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimpleAudioAndSettings;

public class ExampleAudioPlayer : MonoBehaviour
    {
        [Header("AudioData")]
        public AudioData exampleSFXData;
        public AudioSourceManager exampleSFX;

        [Header("TMPro")]
        public Slider slider;
        public TextMeshProUGUI volumeNumber;
        public GameObject soundPlayer;
        public Button enable;
        public Button disable;
        public TextMeshProUGUI resultingVolume;
        public Slider masterVolume;


        private void Start()
        {
            exampleSFX = new AudioSourceManager(exampleSFXData);
            enable.onClick.AddListener(enableObject);
            disable.onClick.AddListener(disableObject);
            slider.value = exampleSFX.GetVolume();
            volumeNumber.text = slider.value + "";
            slider.onValueChanged.AddListener(delegate { updateMasterVolume(); });
            masterVolume.onValueChanged.AddListener(delegate { updateMasterVolume(); });
            resultingVolume.text = exampleSFX.GetVolume() + "";
        }
        private void disableObject()
        {
            exampleSFX.StopAudio();
            soundPlayer.SetActive(false);
        }
        private void enableObject()
        {
            soundPlayer.SetActive(true);
            Debug.Log("I tried");
            exampleSFX.PlayAudio();
        }
        private void updateMasterVolume()
        {
            switch (exampleSFX.GetAudioType())
            {
                case SimpleAudioAndSettings.AudioType.MUSIC:
                    GlobalSettings.globalSettings.musicVolume.SetValue(slider.value);
                    break;
                case SimpleAudioAndSettings.AudioType.SFX:
                    GlobalSettings.globalSettings.SFXVolume.SetValue(slider.value);
                    break;
                case SimpleAudioAndSettings.AudioType.VOICE:
                    GlobalSettings.globalSettings.voiceVolume.SetValue(slider.value);
                    break;
                case SimpleAudioAndSettings.AudioType.DEFAULT:
                    GlobalSettings.globalSettings.masterVolume.SetValue(slider.value);
                    break;
                default:
                    Debug.LogError("Unknown AudioType " + exampleSFX.GetAudioType());
                    break;
            }
            volumeNumber.text = slider.value + "";
            resultingVolume.text = exampleSFX.GetVolume() + "";
        }
    }

