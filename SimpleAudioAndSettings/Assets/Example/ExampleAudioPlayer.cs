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
        exampleSFX.PlayAudio();
    }
    private void updateMasterVolume()
    {
        for (int i = 0; i < GlobalSettings.globalSettings.allVolumes.Count; i++)
        {
            //Debug.Log("My type " + exampleSFX.GetAudioType() + " type checking " + GlobalSettings.globalSettings.allVolumes[i].type);
            if (exampleSFX.GetAudioType() == GlobalSettings.globalSettings.allVolumes[i].type)
            {
                //Debug.Log("Set value to  " + slider.value);
                GlobalSettings.globalSettings.allVolumes[i].SetValue(slider.value);
                break;
            }
        }
        volumeNumber.text = slider.value + "";
        resultingVolume.text = exampleSFX.GetVolume() + "";
    }
}
