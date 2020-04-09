using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Grabber : MonoBehaviour
{
    [SerializeField]
    Color defaultColor = Color.blue, grabColor = Color.red;

    bool grabbing;

    SpriteRenderer sr;
    KLD_DamageTaker damageTaker;
    PlayerController2D controller;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        damageTaker = GameObject.Find("Player").GetComponent<KLD_DamageTaker>();
        controller = damageTaker.transform.GetComponent<PlayerController2D>();
        sr.color = defaultColor;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !damageTaker.isInvulnerable && !controller.getFlatSlideStatus())
        {
            grabbing = true;
            sr.color = grabColor;
            damageTaker.doDamageTaking(DamageType.Grab, transform.GetChild(0), 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageTaker.isInvulnerable && grabbing)
        {
            grabbing = false;
            sr.color = defaultColor;
        }
    }
}
