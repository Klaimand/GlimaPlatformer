using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class KLD_IntroSequence : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup fadeInOnStart;

    [SerializeField]
    private CanvasGroup[] canvasesToFadeInAfterCamChange;

    [SerializeField]
    private GameObject camChangeWall, limousine;

    [SerializeField]
    private CanvasGroup borisCanvas;

    KLD_AudioManager audioManager;
    KLD_Timer timer;

    bool isStoppedOnRamp = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitAndfadeIn());
        audioManager = GameObject.Find("AudioManager").GetComponent<KLD_AudioManager>();
        timer = GameObject.Find("Player").GetComponent<KLD_Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        checkIfStoppedOnRamp();
    }

    private IEnumerator waitAndfadeIn ()
    {
        yield return new WaitForSeconds(6);
        fadeInCanvasInst(fadeInOnStart);
    }




    /*
    public void startRampSlowMo ()
    {
        startSlowMo(0.4f, 2f);
    }

    public void startRampCompleteSlowMo()
    {
        startSlowMo(0f, 0.5f);
    }

    private void startSlowMo(float _speed, float _time)
    {
        StartCoroutine(startSlowMoFade(_speed, _time));
    }

    IEnumerator startSlowMoFade (float _speed, float _time)
    {
        float curTime = _time;
        while (curTime > 0)
        {
            Time.timeScale = curTime / _time;
            curTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = _speed;
        isStoppedOnRamp = true;
    }

    public void stopSlowMo ()
    {
        Time.timeScale = 1f;
    }*/

    private float canvasFadeTime = 0.5f;

    public void fadeInCanvasInst (CanvasGroup canvas)
    {
        StartCoroutine(fadeInCanvas(canvas));
    }

    private IEnumerator fadeInCanvas (CanvasGroup canvas)
    {
        float curTime = 0f;
        while (curTime < canvasFadeTime)
        {
            canvas.alpha = curTime / canvasFadeTime;
            curTime += Time.deltaTime;
            yield return null;
        }
    }

    public void fadeOutCanvasInst(CanvasGroup canvas)
    {
        StartCoroutine(fadeOutCanvas(canvas));
    }

    private IEnumerator fadeOutCanvas(CanvasGroup canvas)
    {
        float curTime = 0f;
        while (curTime < canvasFadeTime)
        {
            canvas.alpha = 1 - (curTime / canvasFadeTime);
            curTime += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 0f;
    }

    public void changeCameraInst ()
    {
        StartCoroutine(changeCamera());
    }

    private IEnumerator changeCamera ()
    {
        GameObject.Find("CM vcam2").GetComponent<CinemachineVirtualCamera>().Priority = 11;

        yield return new WaitForSeconds(2);
        
        foreach (CanvasGroup canvasGroup in canvasesToFadeInAfterCamChange)
        {
            fadeInCanvasInst(canvasGroup);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        GameObject.Find("CM vcam2").GetComponent<CinemachineVirtualCamera>().Priority = 9;

        yield return new WaitForSeconds(1.5f);

        fadeInCanvasInst(borisCanvas);
        Destroy(camChangeWall);
    }

    void checkIfStoppedOnRamp ()
    {
        if (isStoppedOnRamp && Input.GetButton("Fire1"))
        {
            isStoppedOnRamp = false;
            //stopSlowMo();
            audioManager.GetSound("DefenseMatrixIntro").GetSource().Stop();
            audioManager.PlaySound("DefenseMatrix");
        }
    }

    public void startGame ()
    {
        timer.started = true; //starttimer
        timer.updateUI = true;
        //explodecar
        //carsoundeffect
        //changecarsprite
        audioManager.GetSound("DefenseMatrixIntro").GetSource().Stop();
        audioManager.PlaySound("DefenseMatrix");
    }

    public void SetLimousinePetee ()
    {
        limousine.GetComponent<Animator>().SetBool("exploded", true);
    }

}
