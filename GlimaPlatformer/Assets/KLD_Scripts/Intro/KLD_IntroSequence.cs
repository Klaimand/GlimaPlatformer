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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitAndfadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator waitAndfadeIn ()
    {
        yield return new WaitForSeconds(2);
        fadeInCanvasInst(fadeInOnStart);
    }

    public void startSlowMo(float speed)
    {
        Time.timeScale = speed;
    }

    public void stopSlowMo ()
    {
        Time.timeScale = 1f;
    }

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
    }

}
