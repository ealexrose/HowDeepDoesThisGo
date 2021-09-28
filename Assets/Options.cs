using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    public Toggle enableTutorial;


    bool changeVolume = false;



    private void Awake()
    {
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 1f);
            PlayerPrefs.SetFloat("MusicVolume", 1f);
            PlayerPrefs.SetFloat("SFXVolume", 1f);
            PlayerPrefs.SetInt("tutorialEnabled", 1);

        }
        {
            mixer.SetFloat("Master", PlayerPrefs.GetFloat("MasterVolume"));
            Debug.Log( PlayerPrefs.GetFloat("MasterVolume"));

            mixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume"));
            mixer.SetFloat("SoundEffects", PlayerPrefs.GetFloat("SFXVolume"));
            float output = 0f;
            mixer.GetFloat("Master",out output);
            Debug.Log(output);
        }

    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 1f);
            PlayerPrefs.SetFloat("MusicVolume", 1f);
            PlayerPrefs.SetFloat("SFXVolume", 1f);
            PlayerPrefs.SetInt("tutorialEnabled", 1);

        }
        else
        {
            masterVolume.value = Mathf.Clamp(Mathf.Pow(10f, PlayerPrefs.GetFloat("MasterVolume") / 20f), 0.001f, 1f);
            musicVolume.value = Mathf.Clamp(Mathf.Pow(10f, PlayerPrefs.GetFloat("MusicVolume") / 20f), 0.001f, 1f);
            sfxVolume.value = Mathf.Clamp(Mathf.Pow(10f, PlayerPrefs.GetFloat("SFXVolume") / 20f), 0.001f, 1f);
            enableTutorial.isOn = (PlayerPrefs.GetInt("tutorialEnabled") == 1);

        }

    }
    public void SetMasterVolume(float newVolume)
    {

        float setVolume = Mathf.Log10(newVolume) * 20;
        mixer.SetFloat("Master", setVolume);
        PlayerPrefs.SetFloat("MasterVolume", setVolume);

    }

    public void SetMusicVolume(float newVolume)
    {
        float setVolume = Mathf.Log10(newVolume) * 20;
        mixer.SetFloat("Music", setVolume);
        PlayerPrefs.SetFloat("MusicVolume", setVolume);
    }

    public void SetSFXVolume(float newVolume)
    {
        float setVolume = Mathf.Log10(newVolume) * 20;
        mixer.SetFloat("SoundEffects", setVolume);
        PlayerPrefs.SetFloat("SFXVolume", setVolume);
    }

    public void SetTutorial(bool tutorialState)
    {
        PlayerPrefs.SetInt("tutorialEnabled", tutorialState ? 1 : 0);
    }


    public void SelectSound()
    {
        AudioManager.instance.Play("HolePunch" + Random.Range(1, 5).ToString());
    }
    public void PaperSelectSound(bool added)
    {
        if (added)
        {
            AudioManager.instance.Play("Hit1");
        }
        else
        {
            AudioManager.instance.Play("Rip1");
        }

    }
}
