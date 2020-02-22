using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_ControllerTest : MonoBehaviour
{
    public bool jump;
    public bool crouch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getControllerButtons();
    }

    void getControllerButtons ()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            jump = true;
            print("gotjump");
        }
        else
        {
            jump = false;
        }
    }
}
