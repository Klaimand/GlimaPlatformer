using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_HelicoIntro : MonoBehaviour
{
    KLD_IntroSequence introFonc;


    public Transform helicoTransform;

    public GameObject dialogCanvas;
    CanvasGroup dialogCanvasGroup;

    public float canvasDuration = 5f;

    bool dialogClosed = false;

    // Start is called before the first frame update
    void Start()
    {
        introFonc = GameObject.Find("IntroObj").GetComponent<KLD_IntroSequence>();

        dialogCanvasGroup = dialogCanvas.GetComponent<CanvasGroup>();
        dialogCanvasGroup.alpha = 0f;
        dialogCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        linkWindowAndHelicoPos();
    }


    void linkWindowAndHelicoPos ()
    {
        transform.position = new Vector3(transform.position.x, helicoTransform.position.y, 0f);
    }

    public void openWindow ()
    {
        GetComponent<Animator>().SetTrigger("OpenWindow");
    }

    public void spawnDialogInst ()
    {
        StartCoroutine(spawnDialog());
    }

    IEnumerator spawnDialog ()
    {
        if (!dialogClosed) {
            dialogCanvas.SetActive(true);
            introFonc.fadeInCanvasInst(dialogCanvasGroup);
            yield return new WaitForSeconds(canvasDuration);
            closeDialog();
        }
    }

    public void closeDialog ()
    {
        if (!dialogClosed)
        {
            introFonc.fadeOutCanvasInst(dialogCanvasGroup);
            dialogClosed = true;
            StartCoroutine(destroyDialogCanvas());
        }
    }

    IEnumerator destroyDialogCanvas ()
    {
        yield return new WaitForSeconds(2f);
        Destroy(dialogCanvas);
    }

}
