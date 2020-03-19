using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_TestQTE : MonoBehaviour
{
    public float maxPoints;
    private float currentPoints;
    public float pointsPerInput;
    public float pointsLostPerSecond;

    public float joystickLenghtToAdd;

    private bool done;

    private float curXAxisRawValue;
    private float lastXAxisRawValue;

    private SpriteRenderer sr;
    public Color basic, doneColor;

    public enum QteMode
    {
        button,
        joystick
    }

    public QteMode qteMode;


    private GameObject player;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        sr.color = basic;
    }
    
    void Update()
    {
        doCurXValue();
        if (!done)
        {
            addPoints();
            decreasePoints();
            checkEnd();
        }
        doGraphics();
    }

    private void doCurXValue ()
    {
        float xAxisRaw = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(xAxisRaw) >= joystickLenghtToAdd)
        {
            curXAxisRawValue = Mathf.Sign(xAxisRaw);
        }
        else
        {
            curXAxisRawValue = 0f;
        }
    }

    private void addPoints ()
    {
        if ((qteMode == QteMode.button && Input.GetButtonDown("Fire1")) || (qteMode == QteMode.joystick && curXAxisRawValue != lastXAxisRawValue && curXAxisRawValue != 0f))
        {
            currentPoints += pointsPerInput;
            lastXAxisRawValue = 0f;
        }
        if (curXAxisRawValue != 0f)
        {
            lastXAxisRawValue = curXAxisRawValue;
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
        if (currentPoints >= maxPoints && !done)
        {
            done = true;
            currentPoints = maxPoints;
            doEnd();
        }
    }

    private void doEnd ()
    {
        player.GetComponent<PlayerController2D>().cantMove = false;
        player.GetComponent<KLD_DamageTaker>().startInvulnerability();
        Destroy(transform.parent.gameObject);
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
