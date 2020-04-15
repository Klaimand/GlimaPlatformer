using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Parallax : MonoBehaviour
{
    Transform player;

    [SerializeField]
    float parallaxCoef;

    float propStartPos;

    float startPosition;
    float endPosition;
    float deltaPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        endPosition = player.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        doParallax();
    }

    void doParallax ()
    {
        startPosition = endPosition;
        endPosition = player.position.x;
        deltaPosition = endPosition - startPosition;

        transform.position += Vector3.right * deltaPosition * (parallaxCoef);
    }

}
