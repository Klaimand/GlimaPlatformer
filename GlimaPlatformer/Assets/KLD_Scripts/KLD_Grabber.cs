using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Grabber : MonoBehaviour
{
    [SerializeField]
    Color defaultColor, grabColor;

    bool grabbing;

    SpriteRenderer sr;
    KLD_DamageTaker damageTaker;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        damageTaker = GameObject.Find("Player").GetComponent<KLD_DamageTaker>();

        sr.color = defaultColor;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !damageTaker.isInvulnerable)
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
