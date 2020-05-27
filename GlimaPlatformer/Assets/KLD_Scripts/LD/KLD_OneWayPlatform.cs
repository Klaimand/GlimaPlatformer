using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_OneWayPlatform : MonoBehaviour
{
    private Transform player;
    private new BoxCollider2D collider;

    private bool activateCollider;

    [SerializeField]
    private float offset = 0.2f;

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
        if (player.position.y >= (transform.position.y + transform.localScale.y / 2f) + offset)
        {
            activateCollider = true;
        }
        else if (player.position.y < (transform.position.y + transform.localScale.y / 2f) - offset)
        {
            activateCollider = false;
        }
        collider.enabled = activateCollider;
    }
}
