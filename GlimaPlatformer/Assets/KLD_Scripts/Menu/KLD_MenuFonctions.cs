using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_MenuFonctions : MonoBehaviour
{

    bool resumeScreenOpened = false;

    GameObject resumeScreenCanvas;
    Button resumeButton;
    PlayerController2D controller;
    KLD_AudioManager audioManager;

    GameObject endGameCanvas;
    GameObject timeTextObject;
    GameObject timeNameObject;

    GameObject cigsObject;

    KLD_Timer timerComponent;
    KLD_CigarettesAttached cigarettesAttached;

    [SerializeField]
    float segmentRevealDuration, timeBetweenSegment;

    [SerializeField]
    float timePerSig, timeAfterCig, timeAfterCigFound;

    // Start is called before the first frame update
    void Start()
    {
        resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        resumeScreenCanvas = GameObject.Find("ResumeScreenCanvas");
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
        audioManager = GameObject.Find("AudioManager").GetComponent<KLD_AudioManager>();
        resumeScreenCanvas.SetActive(false);
        endGameCanvas = GameObject.Find("EndGameCanvas");

        cigsObject = GameObject.Find("Cigs");
        
        endGameCanvas.SetActive(false);
        timeTextObject = endGameCanvas.transform.GetChild(3).gameObject;
        timeNameObject = endGameCanvas.transform.GetChild(2).gameObject;
        timerComponent = GameObject.Find("Player").GetComponent<KLD_Timer>();

        cigarettesAttached = GameObject.Find("Player").GetComponent<KLD_CigarettesAttached>();


    }

    // Update is called once per frame
    void Update()
    {
        checkResumeScreen();

        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            openEndGameScreen();
            //doTimeRevealInst();
            StartCoroutine(doCigsReveal());
        }*/
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && !resumeScreenOpened)
        {
            openResumeScreen();
        }
    }

    void checkResumeScreen ()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (!resumeScreenOpened)
            {
                openResumeScreen();
            }
            else
            {
                closeResumeScreen();
            }
        }
    }
    
    void openResumeScreen ()
    {
        resumeButton.Select();
        resumeScreenOpened = true;
        controller.SetPause(true);
        Time.timeScale = 0;
        resumeScreenCanvas.SetActive(true);
        audioManager.PlaySound("ClickUI");
    }

    public void closeResumeScreen ()
    {
        resumeScreenOpened = false;
        controller.SetPause(false);
        Time.timeScale = 1;
        resumeScreenCanvas.SetActive(false);
        audioManager.PlaySound("ClickUI");
    }

    public void openEndGameScreen ()
    {
        endGameCanvas.SetActive(true);
    }

    public void doTimeRevealInst ()
    {
        StartCoroutine(doTimeReveal());
    }

    IEnumerator doTimeReveal ()
    {
        for (int i = 0; i < timerComponent.segmentTimes.Count; i++) //8x
        {
            timeNameObject.transform.GetChild(i).gameObject.SetActive(true);

            Text _textToModif = timeTextObject.transform.GetChild(i).GetComponent<Text>();

            float curDuration = 0f;
            float startTime = i == 0 ? 0f : timerComponent.segmentTimes[i - 1];
            float endTime = timerComponent.segmentTimes[i];

            audioManager.PlaySound("TimeCount");

            while (curDuration < segmentRevealDuration)
            {
                _textToModif.text = timerComponent.GetTimeString(Mathf.Lerp(startTime, endTime, curDuration/segmentRevealDuration));

                curDuration += Time.deltaTime;
                yield return null;
            }
            _textToModif.text = timerComponent.GetTimeString(timerComponent.segmentTimes[i]);

            audioManager.PlaySound("TimeBing");

            yield return new WaitForSeconds(timeBetweenSegment);
        }

        StartCoroutine(doCigsReveal());
    }

    IEnumerator doCigsReveal ()
    {
        for (int i = 0; i < cigsObject.transform.childCount; i++)
        {
            GameObject curCig = cigsObject.transform.GetChild(i).gameObject;

            curCig.SetActive(true);

            int curCigState = (int)cigarettesAttached.cigarettes[i].cigaretteState;

            curCig.GetComponent<Animator>().SetInteger("State", curCigState);

            if (curCigState == 2)
            {
                audioManager.PlaySound("CigarettePopNew");
                cigarettesAttached.cigarettes[i].cigaretteState = Cigarette.CigaretteState.fantom;
                cigarettesAttached.doSaveCigHasBeenFound(i); //save
                yield return new WaitForSeconds(timeAfterCigFound);
            }
            else
            {
                audioManager.PlaySound("CigarettePopOld");
                yield return new WaitForSeconds(timeAfterCig);
            }
        }
        //do cig state save
    }

}
