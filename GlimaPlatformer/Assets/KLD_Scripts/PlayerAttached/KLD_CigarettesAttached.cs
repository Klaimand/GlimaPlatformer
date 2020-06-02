using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cigarette
{
    public GameObject cigaretteObj;
    public enum CigaretteState
    {
        red, //not found
        fantom, //found
        found
    }

    public CigaretteState cigaretteState = CigaretteState.red;

}

public class KLD_CigarettesAttached : MonoBehaviour
{


    [SerializeField]
    Sprite redCigaretteSprite, fantomCigaretteSprite;

    [SerializeField]
    int fantomAlpha = 100;
    
    public Cigarette[] cigarettes;


    KLD_AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<KLD_AudioManager>();
        //update playerprefs
        loadCigaretteState();
        updateCigarettesSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void loadCigaretteState ()
    {
        for (int i = 0; i < cigarettes.Length; i++)
        {
            cigarettes[i].cigaretteState = (Cigarette.CigaretteState)PlayerPrefs.GetInt("Cigarette" + i);
        }
    }

    public void resetCigaretteSave()
    {
        for (int i = 0; i < cigarettes.Length; i++)
        {
            PlayerPrefs.SetInt("Cigarette" + i.ToString(), 0);
        }
    }

    void updateCigarettesSprite ()
    {

        foreach (Cigarette _cigarette in cigarettes)
        {
            KLD_Cigarettes _cigaretteScript = _cigarette.cigaretteObj.GetComponent<KLD_Cigarettes>();
            SpriteRenderer _sr = _cigarette.cigaretteObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
            
            if (_cigarette.cigaretteState == Cigarette.CigaretteState.fantom)
            {
                _sr.sprite = fantomCigaretteSprite;
                _sr.color = new Color(1, 1, 1, (float)fantomAlpha / 255f);
            }
            else
            {
                _sr.sprite = redCigaretteSprite;
                _sr.color = new Color(1, 1, 1, 1);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cigarette"))
        {
            int cigaretteIndex = collision.gameObject.GetComponent<KLD_Cigarettes>().cigarettesIndex;
            if (cigarettes[cigaretteIndex].cigaretteState == Cigarette.CigaretteState.red)
            {
                cigarettes[cigaretteIndex].cigaretteState = Cigarette.CigaretteState.found;
                audioManager.PlaySound("CigarettePick");
            }
            else
            {
                audioManager.PlaySound("CigarettePickFantom");
            }

            cigarettes[cigaretteIndex].cigaretteObj.GetComponent<KLD_Cigarettes>().triggerFindAnim();

        }
    }

    public void doSaveCigHasBeenFound (int index)
    {
        PlayerPrefs.SetInt("Cigarette" + index.ToString(), 1);
    }

}
