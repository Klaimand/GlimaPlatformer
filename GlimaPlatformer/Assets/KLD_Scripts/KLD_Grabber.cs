using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Grabber : MonoBehaviour
{
    bool grabbing;
    bool inZone;

    SpriteRenderer sr;
    KLD_DamageTaker damageTaker;
    PlayerController2D controller;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        damageTaker = GameObject.Find("Player").GetComponent<KLD_DamageTaker>();
        controller = damageTaker.transform.GetComponent<PlayerController2D>();
    }

    private void Update()
    {
        animator.SetBool("Grabbing", grabbing);
        waitForQteToLeaveGrab();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !damageTaker.isInvulnerable && !controller.getFlatSlideStatus())
        {
            grabbing = true;
            inZone = true;
            damageTaker.doDamageTaking(DamageType.Grab, transform.GetChild(0), 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageTaker.isInvulnerable && grabbing)
        {
            inZone = false;
        }
    }

    void waitForQteToLeaveGrab ()
    {
        if (!inZone && !controller.cantMove)
        {
            grabbing = false;
        }
    }

}
