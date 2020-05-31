using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_Timer : MonoBehaviour
{

    public bool started = false;

    public List<float> segmentTimes = new List<float>();
    public float totalTime = 0f;

    public float finalTotalTime = 0f;
    bool finished = false;

    public int minutes, seconds;
    public string milli;

    public bool updateUI = true;
    public float noUpdateTimeOnCheckpoint = 2f;

    
    [SerializeField]
    float blinkTime, blinkDuration;
    CanvasGroup timerCanvasGroup;



    Text minutesText;
    Text secondsText;
    Text milliText;


    KLD_MenuFonctions menuFonctions;

    // Start is called before the first frame update
    void Start()
    {
        menuFonctions = GameObject.Find("MenuFunctions").GetComponent<KLD_MenuFonctions>();

        minutesText = GameObject.Find("MinutesTimer").GetComponent<Text>();
        secondsText = GameObject.Find("SecondsTimer").GetComponent<Text>();
        milliText = GameObject.Find("MilliTimer").GetComponent<Text>();

        timerCanvasGroup = GameObject.Find("TimeCanvas").GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!finished && started) {
            addFrameTime();
            updateMinsAndSecs();
        }
        if (updateUI)
        {
            updateTimerText();
        }
    }

    void addFrameTime ()
    {
        totalTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("SegmentCheckpoint"))
        {
            changeSegment();
            collider.gameObject.GetComponent<KLD_Checkpoint>().destroyLinkedCheckpoints();
        }
        else if (collider.gameObject.CompareTag("FinalCheckpoint"))
        {
            endTimer();
            collider.gameObject.GetComponent<KLD_Checkpoint>().destroyLinkedCheckpoints();


            menuFonctions.openEndGameScreen();
            menuFonctions.doTimeRevealInst();

        }
    }

    void changeSegment ()
    {
        segmentTimes.Add(totalTime);
        StartCoroutine(lockUI(noUpdateTimeOnCheckpoint));
        StartCoroutine(blink());
    }

    IEnumerator lockUI (float time)
    {
        updateUI = false;
        yield return new WaitForSeconds(time);
        updateUI = true;
    }

    IEnumerator blink()
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

                timerCanvasGroup.alpha = isVisible ? 1f : 0f;
            }


            blinkElapsedTime += Time.deltaTime;
            totalElapsedTime += Time.deltaTime;
            yield return null;
        }

        timerCanvasGroup.alpha = 1f;
    }

    void endTimer ()
    {
        finished = true;
        segmentTimes.Add(totalTime);
        finalTotalTime = totalTime;
    }

    void updateMinsAndSecs ()
    {
        minutes = Mathf.FloorToInt(totalTime) / 60;
        seconds = Mathf.FloorToInt(totalTime) % 60;
        milli = totalTime.ToString("F3");
        milli = milli.Substring(milli.Length - 3);
        print(milli);
    }

    void updateTimerText ()
    {
        minutesText.text = minutes.ToString() + "'";
        secondsText.text = seconds.ToString("00") + "\"";
        milliText.text = milli;
    }

    public float GetSegmentTime (int segmentIndex)
    {
        float segmentTime = 0f; 

        if (segmentIndex == 0)
        {
            segmentTime = segmentTimes[0];
        }
        else
        {
            segmentTime = segmentTimes[segmentIndex] - segmentTimes[segmentIndex - 1];
        }
        return segmentTime;
    }

    public string GetTimeString (float _segmentTime)
    {
        int _segmentMinutes = Mathf.FloorToInt(_segmentTime) / 60;
        int _segmentSeconds = Mathf.FloorToInt(_segmentTime) % 60;
        string _segmentMilli = _segmentTime.ToString("F3");
        _segmentMilli = _segmentMilli.Substring(_segmentMilli.Length - 3);

        return
            (_segmentMinutes == 0 ? "" : _segmentMinutes.ToString() + "'") +
            (_segmentSeconds.ToString("00") + "\"") +
            (_segmentMilli);
    }


}
