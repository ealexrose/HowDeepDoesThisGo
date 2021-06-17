using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class Options : MonoBehaviour
{
    public AudioMixer mixer;


    public void SetMasterVolume(float newVolume) 
    {
        mixer.SetFloat("Master", Mathf.Log10(newVolume) * 20);

    }

    public void SetMusicVolume(float newVolume)
    {
        mixer.SetFloat("Music", Mathf.Log10(newVolume) * 20);
    }

    public void SetSFXVolume(float newVolume)
    {
        mixer.SetFloat("SoundEffects", Mathf.Log10(newVolume) * 20);
    }


    public void SelectSound() 
    {
        AudioManager.instance.Play("HolePunch" + Random.Range(1, 5).ToString());
    }
}
