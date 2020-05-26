using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_IntroSequence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSlowMo(float speed)
    {
        Time.timeScale = speed;
    }

    public void stopSlowMo ()
    {
        Time.timeScale = 1f;
    }
}
