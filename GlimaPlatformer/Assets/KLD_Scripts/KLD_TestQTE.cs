using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_TestQTE : MonoBehaviour
{
    public float maxPoints;
    private float currentPoints;
    public float pointsPerInput;
    public float pointsLostPerSecond;

    private bool done;

    private SpriteRenderer sr;
    public Color basic, doneColor;

    
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sr.color = basic;
    }
    
    void Update()
    {
        if (!done)
        {
            addPoints();
            decreasePoints();
            checkEnd();
        }
        doGraphics();
    }

    private void addPoints ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            currentPoints += pointsPerInput;
        }
    }

    private void decreasePoints ()
    {
        if (currentPoints > 0f)
        {
            currentPoints -= pointsLostPerSecond * Time.deltaTime;
        }
        else if (currentPoints < 0f)
        {
            currentPoints = 0f;
        }
    }

    private void checkEnd ()
    {
        if (currentPoints >= maxPoints)
        {
            done = true;
            currentPoints = maxPoints;
        }
    }

    private void doGraphics ()
    {
        if (done)
        {
            sr.color = doneColor;
        }
        sr.size = new Vector2((currentPoints * 5f) / maxPoints, 0.5f);
    }
}
