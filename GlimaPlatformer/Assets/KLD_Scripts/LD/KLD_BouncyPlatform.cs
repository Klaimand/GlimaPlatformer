using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_BouncyPlatform : MonoBehaviour
{

    Transform player;
    Rigidbody2D playerRb;

    BoxCollider2D thisCollider;

    [SerializeField]
    private float bounciness, minVel;


    private void Awake()
    {
        thisCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerRb = player.GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        checkTrigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -playerRb.velocity.y * bounciness);
        }
    }

    void checkTrigger ()
    {
        if (player.position.y > transform.position.y + (transform.localScale.y/2f) && Mathf.Abs(playerRb.velocity.y) > minVel)
        {
            thisCollider.isTrigger = true;
        }
        else
        {
            thisCollider.isTrigger = false;
        }
    }

}
