using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneDetector : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathZone")
        {
            transform.position = collision.transform.GetChild(0).position;
            rb.velocity = Vector2.zero;
        }
    }
}
