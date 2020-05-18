using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerAudioInst : MonoBehaviour
{
    private KLD_AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<KLD_AudioManager>();
    }

    public void PlaySound (string soundName)
    {
        audioManager.PlaySound(soundName);
    }

}
