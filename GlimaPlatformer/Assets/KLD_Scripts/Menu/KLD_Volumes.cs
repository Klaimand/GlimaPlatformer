using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class KLD_Volumes : MonoBehaviour
{
    public AudioMixer mainMixer;


    public Slider mainVolumeSlider, musicVolumeSlider, sfxVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        checkIfThisIsFirstLaunch();
        loadVolumePrefs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkIfThisIsFirstLaunch ()
    {
        if (PlayerPrefs.GetInt("FirstLaunch") == 0)
        {
            //this is first launch
            PlayerPrefs.SetInt("FirstLaunch", 1);

            PlayerPrefs.SetFloat("MasterVolume", 0f);
            PlayerPrefs.SetFloat("MusicVolume", -20f);
            PlayerPrefs.SetFloat("SfxVolume", 0f);

        }
    }

    public void changeMainVolume ()
    {
        mainMixer.SetFloat("MasterVolume", (mainVolumeSlider.value * 100f) - 80f);
        PlayerPrefs.SetFloat("MasterVolume", (mainVolumeSlider.value * 100f) - 80f);
    }

    public void changeMusicVolume()
    {
        mainMixer.SetFloat("MusicVolume", (musicVolumeSlider.value * 100f) - 80f);
        PlayerPrefs.SetFloat("MusicVolume", (musicVolumeSlider.value * 100f) - 80f);
    }

    public void changeSfxVolume()
    {
        mainMixer.SetFloat("SfxVolume", (sfxVolumeSlider.value * 100f) - 80f);
        PlayerPrefs.SetFloat("SfxVolume", (sfxVolumeSlider.value * 100f) - 80f);
    }

    public void loadSlidersUIOnPrefs ()
    {
        mainVolumeSlider.value = (PlayerPrefs.GetFloat("MasterVolume") + 80f) / 100f;
        musicVolumeSlider.value = (PlayerPrefs.GetFloat("MusicVolume") + 80f) / 100f;
        sfxVolumeSlider.value = (PlayerPrefs.GetFloat("SfxVolume") + 80f) / 100f;
    }

    void loadVolumePrefs ()
    {
        mainMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        mainMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        mainMixer.SetFloat("SfxVolume", PlayerPrefs.GetFloat("SfxVolume"));
    }

}
