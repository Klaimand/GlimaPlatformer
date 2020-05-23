using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_Timer : MonoBehaviour
{
    public List<float> segmentTimes = new List<float>();
    public float totalTime = 0f;

    public int minutes, seconds;
    public string milli;

    private bool updateUI = true;
    public float noUpdateTimeOnCheckpoint = 2f;


    Text minutesText;
    Text secondsText;
    Text milliText;

    // Start is called before the first frame update
    void Start()
    {
        minutesText = GameObject.Find("MinutesTimer").GetComponent<Text>();
        secondsText = GameObject.Find("SecondsTimer").GetComponent<Text>();
        milliText = GameObject.Find("MilliTimer").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        addFrameTime();
        updateMinsAndSecs();
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
    }

    void changeSegment ()
    {
        segmentTimes.Add(totalTime);
        StartCoroutine(lockUI(noUpdateTimeOnCheckpoint));
    }

    IEnumerator lockUI (float time)
    {
        updateUI = false;
        yield return new WaitForSeconds(time);
        updateUI = true;
    }

    void updateMinsAndSecs ()
    {
        minutes = Mathf.FloorToInt(totalTime) / 60;
        seconds = Mathf.FloorToInt(totalTime) % 60;
        milli = totalTime.ToString("F3");
        milli = milli.Substring(milli.Length - 3);
    }

    void updateTimerText ()
    {
        minutesText.text = minutes.ToString() + ":";
        secondsText.text = seconds.ToString("00") + ":";
        milliText.text = milli;
    }


}
