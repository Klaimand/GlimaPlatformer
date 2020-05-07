using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_StairWayPlatform : MonoBehaviour
{
    [SerializeField]
    private bool isStairToTheLeft;

    BoxCollider2D collider;
    PlayerController2D controller;
    bool colliderEnabled;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        doCollider();
    }

    void doCollider ()
    {
        if ((controller.getVirtualXAxis() > 0.03f && isStairToTheLeft) || (controller.getVirtualXAxis() < -0.03f && !isStairToTheLeft))
        {
            colliderEnabled = true;
        }
        else
        {
            colliderEnabled = false;
        }
        collider.enabled = colliderEnabled;
    }
}
