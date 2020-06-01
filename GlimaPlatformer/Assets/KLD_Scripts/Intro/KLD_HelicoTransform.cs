using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KLD_HelicoTransform : MonoBehaviour
{

    public UnityEvent doOnDownFinish;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeEvent ()
    {
        doOnDownFinish.Invoke();
    }

}
