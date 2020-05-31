using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_DestructibleWall : MonoBehaviour
{

    private bool destroyed = false;


    PlayerController2D controller;
    BoxCollider2D[] colliders;

    private void Awake()
    {
        colliders = GetComponents<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!destroyed && collision.gameObject.CompareTag("Player"))
        {
            if (controller.transform.position.x < transform.position.x)
            {
                Destroy(gameObject);
            }
        }
    }
}
