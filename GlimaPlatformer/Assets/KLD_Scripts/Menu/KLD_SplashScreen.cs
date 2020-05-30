using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class KLD_SplashScreen : MonoBehaviour
{
    [SerializeField]
    float blinkTime, blinkDuration;

    bool keyWasPressed;

    [SerializeField]
    Animator AnyKeyTextAnimator, BlackFadeAnimator;
    [SerializeField]
    Text AnyKeyText;

    public UnityEvent OnAnyKeyPress;
    public UnityEvent OnFadeFinish;

    // Start is called before the first frame update
    void Awake()
    {
        keyWasPressed = false;
    }

    private void Start()
    {
        BlackFadeAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkIfAKeyIsPressed();
    }

    void checkIfAKeyIsPressed ()
    {
        if (Input.anyKey && !keyWasPressed)
        {
            keyWasPressed = true;
            StartCoroutine(startBlackFade());
            AnyKeyTextAnimator.SetBool("KeyPressed", true);
            StartCoroutine(blink());
            OnAnyKeyPress.Invoke();
        }
    }

    IEnumerator startBlackFade ()
    {
        yield return new WaitForSeconds(blinkTime - 1f);
        BlackFadeAnimator.enabled = true;
    }

    IEnumerator blink ()
    {
        float totalElapsedTime = 0f;
        float blinkElapsedTime = 0f;
        bool isVisible = false;
        
        while (totalElapsedTime < blinkDuration)
        {
            if (blinkElapsedTime > blinkTime)
            {
                isVisible = !isVisible;
                blinkElapsedTime = 0f;

                AnyKeyText.enabled = isVisible;
            }


            blinkElapsedTime += Time.deltaTime;
            totalElapsedTime += Time.deltaTime;
            yield return null;
        }
        callOnFadeFinish();
    }

    void callOnFadeFinish ()
    {
        //SceneManager.LoadScene("KLD_MenuPrincipal");
        OnFadeFinish.Invoke();
    }
}
