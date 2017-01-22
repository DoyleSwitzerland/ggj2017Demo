using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagement : MonoBehaviour {

    public Slider volumeSlider;
    public AudioSource volumeAudio;

    public void VolumeController()
    {
        volumeSlider.value = volumeAudio.volume;
    }
}
