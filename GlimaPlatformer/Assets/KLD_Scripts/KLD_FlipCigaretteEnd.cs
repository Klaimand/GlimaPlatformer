using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_FlipCigaretteEnd : MonoBehaviour
{
    PlayerController2D controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        checkFlip();
    }

    void checkFlip ()
    {
        float direction = 0f;
        if (controller.playerState == PlayerController2D.PlayerState.BlowedFalling || controller.playerState == PlayerController2D.PlayerState.BlowedAscending)
        {
            direction = !controller.FlipXInst ? -1f : 1f;
        }
        else
        {
            direction = controller.FlipXInst ? -1f : 1f;
        }
        transform.localPosition = new Vector3(direction * Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
    }
}
