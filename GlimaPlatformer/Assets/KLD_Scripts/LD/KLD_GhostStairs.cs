using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_GhostStairs : MonoBehaviour
{
    BoxCollider2D collider;
    CapsuleCollider2D playercollider;
    Rigidbody2D playerrb;

    float yAxis;
    [SerializeField]
    float axisThreshold;
    [SerializeField]
    bool toTheLeft;
    [SerializeField]
    private float offset;
    bool enabled;

    PlayerController2D controller;
    Transform origin;
    Transform origin2;
    Transform player;
    

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        origin = transform.GetChild(0);
        origin2 = transform.GetChild(1);
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
        playercollider = GameObject.Find("Player").GetComponent<CapsuleCollider2D>();
        playerrb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        player = controller.transform;
    }

    // Update is called once per frame
    void Update()
    {
        yAxis = Input.GetAxisRaw("Vertical");
        checkGhost();
    }


    void checkGhost()
    {
        Vector3 thisVector = transform.GetChild(1).position - transform.GetChild(0).position;
        Debug.DrawLine(transform.GetChild(1).position, transform.GetChild(0).position, Color.blue);

        Vector3 thisVectorOffset = (transform.GetChild(1).position + Vector3.up * offset) - (transform.GetChild(0).position + Vector3.up * offset);
        Debug.DrawLine(transform.GetChild(0).position + Vector3.up * offset, transform.GetChild(0).position + Vector3.up * offset + thisVector, Color.green);

        Vector3 stairToPlayerVector = player.position - transform.GetChild(0).position;
        
        float xp = thisVector.x * stairToPlayerVector.y - thisVector.y * stairToPlayerVector.x;
        float xo = thisVectorOffset.x * stairToPlayerVector.y - thisVectorOffset.y * stairToPlayerVector.x;

        if (xp > 0)
        {
            //print("above");
            if (controller.getGroundStatus() && !controller.getStairsStatus()) {
                if (yAxis > axisThreshold)
                {
                    enabled = true;
                }
                else
                {
                    enabled = false;
                }
            }
            else if (!controller.getGroundStatus())
            {
                if (xo > 0)
                {
                    enabled = true;
                }
                else
                {
                    enabled = false;
                }
                
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

        collider.enabled = enabled;

        //y = ax + b

    }


}
