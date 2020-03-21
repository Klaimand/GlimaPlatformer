using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_OneWayPlatform : MonoBehaviour
{
    private Transform player;
    private BoxCollider2D collider;

    private bool activateCollider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        doColliderActivation();   
    }

    void doColliderActivation ()
    {
        if (player.position.y >= (transform.position.y + transform.localScale.y / 2f) + 0.1f)
        {
            activateCollider = true;
        }
        else if (player.position.y < (transform.position.y + transform.localScale.y / 2f) - 0.1f)
        {
            activateCollider = false;
        }
        collider.enabled = activateCollider;
    }
}
