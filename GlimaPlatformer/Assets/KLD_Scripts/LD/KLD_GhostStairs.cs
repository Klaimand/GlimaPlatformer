using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_GhostStairs : MonoBehaviour
{
    BoxCollider2D collider;

    float yAxis;
    [SerializeField]
    float axisThreshold;
    [SerializeField]
    bool toTheLeft;
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

    }


}
