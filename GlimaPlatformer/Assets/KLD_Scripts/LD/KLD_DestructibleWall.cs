using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_DestructibleWall : MonoBehaviour
{

    private bool destroyed = false;


    PlayerController2D controller;
    BoxCollider2D[] colliders;

    public Animator placoAnimator;

    KLD_AudioManager audioManager;

    private void Awake()
    {
        colliders = GetComponents<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<KLD_AudioManager>();
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!destroyed && collision.gameObject.CompareTag("Player"))
        {
            if (controller.transform.position.x < transform.position.x)
            {
                audioManager.PlaySound("WallDestroy");
                placoAnimator.SetTrigger("Destroy");
                Destroy(placoAnimator.gameObject, 0.5f);
                Destroy(gameObject);//, 0.5f);
            }
        }
    }
}
