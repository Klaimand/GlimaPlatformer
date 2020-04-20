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

    public List<SpriteRenderer> SpriteRenderers = new List<SpriteRenderer>();

    [SerializeField]
    bool isVisiblee;

    // Start is called before the first frame update
    void Start()
    {
        getRenderers();
        player = GameObject.Find("Player").transform;
        endPosition = player.position.x;
    }

    private void Update()
    {
        checkIfASrIsVisible();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        doParallax();
    }

    void getRenderers ()
    {
        if (TryGetComponent(out SpriteRenderer sr))
        {
            SpriteRenderers.Add(sr);
        }
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out SpriteRenderer childsr))
            {
                SpriteRenderers.Add(childsr);
            }
        }
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

    void doParallax()
    {
            startPosition = endPosition;
            endPosition = player.position.x;
            deltaPosition = endPosition - startPosition;

        if (isVisiblee)
        {
            transform.position += Vector3.right * deltaPosition * (parallaxCoef);
        }
    }

}
