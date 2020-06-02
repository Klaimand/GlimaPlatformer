using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup group;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0f;

    private AudioSource source;

    public void SetSource (AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.outputAudioMixerGroup = group;
    }

    public AudioSource GetSource ()
    {
        return source;
    }

    public void Play ()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume/2f, randomVolume/2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

}


public class KLD_AudioManager : MonoBehaviour
{
    [SerializeField]
    bool mainMenu = false;

    [SerializeField]
    Sound[] sounds;

    PlayerController2D controller;

    [SerializeField]
    string[] soundsToPlayOnStart;

    private void Start()
    {
        
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.parent = transform;
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

        foreach (string sound in soundsToPlayOnStart)
        {
            PlaySound(sound);
        }

        if (!mainMenu)
        {
            controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
            GetSound("DefenseMatrixIntro").GetSource().loop = true;
            GetSound("DefenseMatrix").GetSource().loop = true;
        }
        else if (mainMenu)
        {
            GetSound("RingsOfJupiter").GetSource().loop = true;
        }
    }

    public void PlaySound (string _name)
    {
        bool foundsmthng = false;
        foreach (Sound sound in sounds)
        {
            if (sound.name == _name)
            {
                sound.Play();
                foundsmthng = true;
                //return;
            }
        }
        if (!foundsmthng)
        Debug.LogWarning("No found sound '" + _name + "'");
    }

    public Sound GetSound (string _name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == _name)
            {
                return sound;
            }
        }

        Debug.LogError("Not able to get sound '" + _name + "'");
        return null;

    }

    public void PlayQte ()
    {
        KLD_TestQTE curQteScript = GameObject.Find("QTEfill").GetComponent<KLD_TestQTE>();
        float curFillPercentage = curQteScript.currentPoints / curQteScript.maxPoints;

        GetSound("QteClick").GetSource().pitch = 1f + curFillPercentage;
        GetSound("QteClick").GetSource().Play();
    }


    private void Update()
    {
        if (!mainMenu) {
            doWallSlideSound();
            doFlatSlideSound();
            doSlopeSlideSound();
            doStandSlideSound();
        }
    }

    public void FadeOutInst (AudioSource _source, float time)
    {
        StartCoroutine(FadeOut(_source, time));
    }

    IEnumerator FadeOut (AudioSource _source, float time)
    {
        float curTime = 0f;
        float startVolume = _source.volume;

        while (curTime < time)
        {
            _source.volume = Mathf.Lerp(startVolume, 0f, curTime/time);
            curTime += Time.deltaTime;
            yield return null;
        }
        _source.volume = 0f;

        _source.Stop();
        
    }


    void doWallSlideSound()
    {
        if (controller.getWallSlideStatus() && !GetSound("WallSlide").GetSource().isPlaying)
        {
            PlaySound("WallSlide");
        }
        else if (!controller.getWallSlideStatus() && GetSound("WallSlide").GetSource().isPlaying)
        {
            FadeOutInst(GetSound("WallSlide").GetSource(), 0.1f);
        }
        
    }

    void doFlatSlideSound()
    {
        if (controller.getFlatSlideStatus() && !GetSound("FlatSlide").GetSource().isPlaying)
        {
            PlaySound("FlatSlide");
        }
        else if (!controller.getFlatSlideStatus() && GetSound("FlatSlide").GetSource().isPlaying)
        {
            FadeOutInst(GetSound("FlatSlide").GetSource(), 0.1f);
        }
        
        GetSound("FlatSlide").GetSource().volume = (controller.getFlatSlideSpeedPercentage() / 0.7f) * GetSound("FlatSlide").volume - 0.1f;
    }

    void doSlopeSlideSound()
    {
        if (controller.getSlopeSlideStatus() && !GetSound("SlopeSlide").GetSource().isPlaying)
        {
            PlaySound("SlopeSlide");
        }
        else if (!controller.getSlopeSlideStatus() && GetSound("SlopeSlide").GetSource().isPlaying)
        {
            FadeOutInst(GetSound("SlopeSlide").GetSource(), 0.1f);
        }
    }

    void doStandSlideSound()
    {
        if (controller.getSlopeStandStatus() && !GetSound("StandSlide").GetSource().isPlaying)
        {
            PlaySound("StandSlide");
        }
        else if (!controller.getSlopeStandStatus() && GetSound("StandSlide").GetSource().isPlaying)
        {
            FadeOutInst(GetSound("StandSlide").GetSource(), 0.1f);
        }
    }

}
