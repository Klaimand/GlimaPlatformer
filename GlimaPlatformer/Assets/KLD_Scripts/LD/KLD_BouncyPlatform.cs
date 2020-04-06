using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_BouncyPlatform : MonoBehaviour
{

    Transform player;
    Rigidbody2D playerRb;
    PlayerController2D controller;

    BoxCollider2D thisCollider;

    [SerializeField]
    private float bounciness, minVel, maxVel, bouncinessAddedWhenButtonIsPressed;
    
    private void Awake()
    {
        thisCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerRb = player.GetComponent<Rigidbody2D>();
        controller = player.GetComponent<PlayerController2D>();
    }
    
    void Update()
    {
        checkTrigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float bouncinessToAdd = Input.GetButton("Fire1") ? bouncinessAddedWhenButtonIsPressed : 0f;
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Min(-playerRb.velocity.y * (bounciness + bouncinessToAdd), maxVel));
            controller.SetLastJumpIsBounce(true); //not working...
        }
    }

    void checkTrigger ()
    {
        if (player.position.y > transform.position.y && Mathf.Abs(playerRb.velocity.y) > minVel)
        {
            thisCollider.isTrigger = true;
        }
        else
        {
            thisCollider.isTrigger = false;
        }
    }

}
