using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Timer : MonoBehaviour
{
    public List<float> segmentTimes = new List<float>();
    public float totalTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        addFrameTime();
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
        }
    }

    void changeSegment ()
    {
        segmentTimes.Add(totalTime);
    }

}
