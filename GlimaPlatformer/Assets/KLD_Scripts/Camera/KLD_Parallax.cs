using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Parallax : MonoBehaviour
{
    Transform player;

    [SerializeField]
    float parallaxCoef;

    float propStartPos;

    /*
    float startPosition;
    float endPosition;
    float deltaPosition;
    */

    Vector2 startPosition, endPosition, deltaPosition;

    public SpriteRenderer[] SpriteRenderers;

    [SerializeField]
    bool isVisiblee, doVerticalParallax = false;
    
    void Start()
    {
        getRenderers();
        player = GameObject.Find("Player").transform;
        //endPosition = player.position.x;
        endPosition = player.position;
    }

    private void Update()
    {
        checkIfASrIsVisible();
    }
    
    void FixedUpdate()
    {
        doParallax();
    }

    void getRenderers ()
    {
        SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void checkIfASrIsVisible ()
    {
        bool isVisible = false;
        foreach (SpriteRenderer sr in SpriteRenderers)
        {
            if (sr.isVisible)
            {
                isVisible = true;
            }
        }
        isVisiblee = isVisible;
    }

    /*
    void doParallax()
    {
            startPosition = endPosition;
            endPosition = player.position.x;
            deltaPosition = endPosition - startPosition;

        if (isVisiblee)
        {
            transform.position += Vector3.right * deltaPosition * (parallaxCoef);
        }
    }*/

    void doParallax()
    {
        startPosition = endPosition;
        endPosition = player.position;
        deltaPosition = endPosition - startPosition;

        if (isVisiblee)
        {
            if (!doVerticalParallax)
            {
                transform.position += new Vector3(deltaPosition.x, 0f, 0f) * (parallaxCoef);
            }
            else
            {
                transform.position += (Vector3)deltaPosition * parallaxCoef;
            }
        }
    }

}
