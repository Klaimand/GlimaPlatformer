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
    public UnityEvent OnGroundRecovery;
    public UnityEvent OnFlatSlide;
    public UnityEvent OnSlopeSlide;
    public UnityEvent OnDamageTaking;
    public UnityEvent OnGroundHitdead;
    public UnityEvent OnDoorOpen;
    public UnityEvent OnBouncyPlatformJump;
    public UnityEvent OnQTEPress;
    public UnityEvent OnQTEComplete;

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
    }

    public void InvokeGroundRecovery ()
    {
        OnGroundRecovery.Invoke();
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

    public void InvokeGroundHitDead ()
    {
        OnGroundHitdead.Invoke();
    }

    public void InvokeDoorOpening ()
    {
        OnDoorOpen.Invoke();
    }

    public void InvokeBouncyPlatformJump ()
    {
        OnBouncyPlatformJump.Invoke();
    }

    public void InvokeQTEPress ()
    {
        OnQTEPress.Invoke();
    }

    public void InvokeQTEComplete()
    {
        OnQTEComplete.Invoke();
    }

}
