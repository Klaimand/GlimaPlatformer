using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_GhostStairs : MonoBehaviour
{
    BoxCollider2D thisCollider;

    float yAxis;
    [SerializeField]
    float axisThreshold = 0f;
    [SerializeField]
    bool toTheLeft = false;
    [SerializeField]
    private float offset = 0f;
    new bool enabled;
    bool wasAboveOffset;

    [SerializeField]
    bool drawRays;


    PlayerController2D controller;
    Transform player;
    

    private void Awake()
    {
        thisCollider = GetComponent<BoxCollider2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
        player = controller.transform;
    }

    // Update is called once per frame
    void Update()
    {
        yAxis = Input.GetAxisRaw("Vertical");
        checkGhost();
        resetAboveOffset();
    }


    void checkGhost()
    {
        transform.GetChild(2).position = transform.GetChild(0).position + Vector3.up * offset;
        transform.GetChild(3).position = transform.GetChild(1).position + Vector3.up * offset;

        Vector3 thisVector = transform.GetChild(1).position - transform.GetChild(0).position;
        
        Vector3 thisVectorOffset = transform.GetChild(3).position - transform.GetChild(2).position;

        if (drawRays) {
            Debug.DrawLine(transform.GetChild(1).position, transform.GetChild(0).position, Color.blue);
            Debug.DrawLine(transform.GetChild(2).position, transform.GetChild(3).position, Color.green);
        }

        Vector3 stairToPlayerVector = player.position - transform.GetChild(0).position;
        Vector3 stairToPlayerVectorOffset = player.position - transform.GetChild(2).position;

        float xp = thisVector.x * stairToPlayerVector.y - thisVector.y * stairToPlayerVector.x;
        float xo = thisVectorOffset.x * stairToPlayerVectorOffset.y - thisVectorOffset.y * stairToPlayerVectorOffset.x;

        if (!toTheLeft)
        {
            xp = -xp;
            xo = -xo;
        }

        if (xp > 0)
        {
            //print("above");
            if (controller.getGroundStatus() && !controller.getStairsStatus()) {
                if (yAxis > axisThreshold || player.position.y > transform.GetChild(1).position.y)
                {
                    enabled = true;
                }
                else
                {
                    enabled = false;
                }
            }
            else if (!controller.getGroundStatus() && !controller.getStairsStatus() && !wasAboveOffset)
            {
                if (xo > 0)
                {
                    wasAboveOffset = true;
                    enabled = true;
                }
                else
                {
                    enabled = false;
                }
            }
            else if (controller.getStairsStatus())
            {
                enabled = true;
            }
        }

        else if (xp < 0)
        {
            //print("below");
            enabled = false;
        }

        else
        {
            //print("same line");
            enabled = false;
        }

        thisCollider.enabled = enabled;

    }

    void resetAboveOffset ()
    {
        if (controller.getGroundStatus())
        {
            wasAboveOffset = false;
        }
    }


}
