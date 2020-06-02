using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject endGameReturn;

    KLD_Timer timerComponent;
    KLD_CigarettesAttached cigarettesAttached;


    KLD_IntroSequence introSequence;

    public GameObject dialogCanvas;
    CanvasGroup dialogCanvasGroup;
    bool dialogClosed = false;

    [SerializeField]
    float segmentRevealDuration, timeBetweenSegment;

    [SerializeField]
    float timePerSig, timeAfterCig, timeAfterCigFound;

    public Transform helicoImmeublesSpawn;
    public GameObject helicoImmeublesPrefab;


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

        introSequence = GameObject.Find("IntroObj").GetComponent<KLD_IntroSequence>();

        dialogCanvasGroup = dialogCanvas.GetComponent<CanvasGroup>();
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
        resumeScreenCanvas.transform.GetChild(2).gameObject.SetActive(true);
        resumeScreenCanvas.transform.GetChild(3).gameObject.SetActive(false);
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
        endGameReturn.SetActive(true);
        endGameReturn.GetComponent<Button>().Select();
    }


    public void loadMainmenu ()
    {
        SceneManager.LoadScene("KLD_MenuPrincipal");
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        //SceneManager.LoadScene(0);
    }

    public void spawnFirstDialog ()
    {
        spawnTextInst(false, 7f, 0);
    }

    public void openDialog1 ()
    {
        spawnTextInst(true, 4f, 1);
    }

    void spawnTextInst (bool up, float duration, int textIndex)
    {
        StartCoroutine(spawnText(up, duration, textIndex));
    }

    IEnumerator spawnText (bool up, float duration, int textIndex)
    {
        if (!up)
        {
            dialogCanvas.transform.GetChild(0).gameObject.SetActive(true);
            dialogCanvas.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (up)
        {
            dialogCanvas.transform.GetChild(0).gameObject.SetActive(false);
            dialogCanvas.transform.GetChild(1).gameObject.SetActive(true);
        }

        for (int i = 0; i < dialogCanvas.transform.childCount - 1; i++)
        {
            if (i == textIndex)
            {
                dialogCanvas.transform.GetChild(2).GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                dialogCanvas.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
            }
        }

        introSequence.fadeInCanvasInst(dialogCanvasGroup);

        yield return new WaitForSeconds(duration);

        closeDialog(textIndex);
    }

    private List<int> closedDialogIndexes = new List<int>();

    public void closeDialog (int dialogIndex)
    {
        if (!closedDialogIndexes.Contains(dialogIndex))
        {
            closedDialogIndexes.Add(dialogIndex);
            introSequence.fadeOutCanvasInst(dialogCanvasGroup);
        }
    }

    public void spawnHelicoImmeubles ()
    {
        Instantiate(helicoImmeublesPrefab, helicoImmeublesSpawn.position, Quaternion.identity);
    }

}
