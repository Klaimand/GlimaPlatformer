using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KLD_PlayerEvents : MonoBehaviour
{
    
    public UnityEvent OnJump;
    public UnityEvent OnWallJump;
    public UnityEvent OnSlopeJump;
    
    public void InvokeJump ()
    {
        OnJump.Invoke();
    }

    public void InvokeWallJump()
    {
        OnWallJump.Invoke();
    }

    public void InvokeSlopeJump()
    {
        OnSlopeJump.Invoke();
    }

}
