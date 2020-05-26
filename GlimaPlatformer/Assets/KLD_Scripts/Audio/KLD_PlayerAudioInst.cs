using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerAudioInst : MonoBehaviour
{
    private KLD_AudioManager audioManager;

    bool pif = false;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<KLD_AudioManager>();
    }

    public void PlaySound (string soundName)
    {
        audioManager.PlaySound(soundName);
        //print("Played : " + soundName);
    }

    public void PlayRightFoot ()
    {
        string stepToPlay = pif ? "Step02" : "Step01";
        pif = !pif;
        audioManager.PlaySound(stepToPlay);

        //print("Played : " + stepToPlay);
    }

    public void PlayRightFootCrouch()
    {
        string stepToPlay = pif ? "CrouchStep02" : "CrouchStep01";
        pif = !pif;
        audioManager.PlaySound(stepToPlay);

        //print("Played : " + stepToPlay);
    }

    public void PlayQteInst ()
    {
        audioManager.PlayQte();
    }

}
