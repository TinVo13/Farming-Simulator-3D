using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlay : MonoBehaviour
{
    public GameObject music;
    private AudioSource audio;
    private float musicVolume = 0f;
    [SerializeField]
    private Slider Slider;
    private void Start()
    {
        music = GameObject.FindWithTag("GameMusic");
        audio = music.GetComponent<AudioSource>();

        //set volume
        musicVolume = PlayerPrefs.GetFloat("volume");
        audio.volume = musicVolume;
        Slider.value = musicVolume;
    }
    private void Update()
    {
        audio.volume = musicVolume;
        PlayerPrefs.SetFloat("volume", musicVolume);
    }
    public void VolumeUpdate(float volume)
    {
        musicVolume = volume;
    }
    public void MusicReset()
    {
        PlayerPrefs.DeleteKey("volume");
        audio.volume = 1;
        Slider.value = 1;
    }
}
