using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;

    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }
    
    public void SetVolume(float vol)
    {
        if(vol < 1)
        {
            vol = 0.001f;
        }

        RefreshSlider(vol);
        PlayerPrefs.SetFloat("SavedMasterVolume", vol);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(vol / 100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void RefreshSlider(float vol)
    {
        soundSlider.value = vol;
    }
}
