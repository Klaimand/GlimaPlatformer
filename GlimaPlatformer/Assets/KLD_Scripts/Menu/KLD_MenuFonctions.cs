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

    // Start is called before the first frame update
    void Start()
    {
        resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        resumeScreenCanvas = GameObject.Find("ResumeScreenCanvas");
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
        resumeScreenCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkResumeScreen();
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
    }

    public void closeResumeScreen ()
    {
        resumeScreenOpened = false;
        controller.SetPause(false);
        Time.timeScale = 1;
        resumeScreenCanvas.SetActive(false);
    }


}
