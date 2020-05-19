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
    Sound[] sounds;

    PlayerController2D controller;

    private void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.parent = transform;
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound (string _name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == _name)
            {
                sound.Play();
                return;
            }
        }

        Debug.LogWarning("No found sound '" + _name + "'");
    }

    Sound GetSound (string _name)
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

    private void Update()
    {
        doFlatSlideSound();
        doSlopeSlideSound();
        doStandSlideSound();
    }

    void doFlatSlideSound()
    {
        if (controller.getFlatSlideStatus() && !GetSound("FlatSlide").GetSource().isPlaying)
        {
            PlaySound("FlatSlide");
        }
        else if (!controller.getFlatSlideStatus() && GetSound("FlatSlide").GetSource().isPlaying)
        {
            GetSound("FlatSlide").GetSource().Stop();
        }

        GetSound("FlatSlide").GetSource().volume = controller.getFlatSlideSpeedPercentage();
    }

    void doSlopeSlideSound()
    {
        if (controller.getSlopeSlideStatus() && !GetSound("SlopeSlide").GetSource().isPlaying)
        {
            PlaySound("SlopeSlide");
        }
        else if (!controller.getSlopeSlideStatus() && GetSound("SlopeSlide").GetSource().isPlaying)
        {
            GetSound("SlopeSlide").GetSource().Stop();
        }
    }

    void doStandSlideSound()
    {
        if (controller.getSlopeStandStatus() && !GetSound("StandSlide").GetSource().isPlaying)
        {
            PlaySound("StandSlide");
            print("played standslide sound");
        }
        else if (!controller.getSlopeStandStatus() && GetSound("StandSlide").GetSource().isPlaying)
        {
            GetSound("StandSlide").GetSource().Stop();
        }
    }

}
