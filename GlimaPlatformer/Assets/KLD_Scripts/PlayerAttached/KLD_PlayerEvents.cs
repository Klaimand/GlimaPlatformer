using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KLD_PlayerEvents : MonoBehaviour
{
    
    public UnityEvent OnJump;
    public UnityEvent OnWallJump;
    public UnityEvent OnSlopeJump;
    public UnityEvent OnStandSlopeJump;
    public UnityEvent OnFlatSlide;
    public UnityEvent OnSlopeSlide;
    public UnityEvent OnDamageTaking;
    public UnityEvent OnDoorOpen;
    public UnityEvent OnQTEPress;

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

    public void InvokeStandSlopeJump ()
    {
        OnStandSlopeJump.Invoke();
        print("standslopejump");
    }

    public void InvokeFlatSlide()
    {
        OnFlatSlide.Invoke();
    }

    public void InvokeSlopeSlide()
    {
        OnSlopeSlide.Invoke();
    }

    public void InvokeDamageTaking ()
    {
        OnDamageTaking.Invoke();
    }

    public void InvokeDoorOpening ()
    {
        OnDoorOpen.Invoke();
    }

    public void InvokeQTEPress ()
    {
        OnQTEPress.Invoke();
    }

}
