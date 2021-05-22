using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSlider : MonoBehaviour
{

    public string audioGroup;

    public void SetVolume(float volume) 
    {
        AudioManager.instance.SetVolume(audioGroup, volume);
    }
}
