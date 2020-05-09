using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    PlayerController2D controller;

    private float shakeTimeRemaining;
    private float shakePower;
    private float shakeFadeTime;
    private float shakeRotation;

    public float rotationMultiplier = 15f;
    public float MinimumShake = 0.1f;
    public float MaximumShake = 1f;
    public float MinimumRotation = 1f;
    public float MaximumRotation = 1f;
    
    #region ShakeValues

    [Header("Jumps")]
    public float WallJumpLenght;
    public float WallJumpPower;
    [Space(5)]
    public float SlopeJumpLenght;
    public float SlopeJumpPower;
    [Space(5)]
    public float StandSlopeJumpLenght;
    public float StandSlopeJumpPower;
    [Space(5)]
    public float BouncyPlatformShakeLenght;
    public float BouncyPlatformShakePower;

    //public float SlopeSlideLenght;
    [Header("Slides")]
    public float SlopeSlidePower;
    [Space(5)]
    //public float FlatSlideLenght;
    public float FlatSlidePower;
    [Space(5)]
    public float SlopeStandPower;

    [Header("Others")]
    public float DamageTakingLenght;
    public float DamageTakingPower;
    [Space(5)]
    public float GroundHitDeadLenght;
    public float GroundHitDeadPower;
    [Space(5)]
    public float DoorOpenLenght;
    public float DoorOpenPower;
    [Space(5)]
    public float QTEPressLenght;
    public float QTEPressPower;

    #endregion

    Transform mainCamera;


    void Start()
    {
        mainCamera = Camera.main.transform;
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
    }

    void Update()
    {
        checkSlide();
    }
    private void LateUpdate()
    {
        if(shakeTimeRemaining > 0)
        {
            shakeTimeRemaining = Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            mainCamera.position += new Vector3(xAmount, yAmount, 0f);
            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
           
        }

        mainCamera.rotation = Quaternion.Euler(0f, 0f, shakeRotation*Random.Range(MinimumRotation, MaximumRotation));
    }
    
    private void StartShake(float lenght, float power)
    {
        //print("launched shake lenght : " + lenght + " Power : " + power);
        shakeTimeRemaining = lenght;
        shakePower = power;

        shakeFadeTime = power / lenght;

        shakeRotation = power * rotationMultiplier;

    }

    void checkSlide ()
    {
        if (controller.getFlatSlideStatus())
        {
            //print(controller.getFlatSlideSpeedPercentage());
            StartShake(Time.deltaTime, Mathf.Abs(FlatSlidePower * controller.getFlatSlideSpeedPercentage()));
        }
        else if (controller.getSlopeSlideStatus())
        {
            StartShake(Time.deltaTime, SlopeSlidePower);
        }
        else if (controller.getSlopeStandStatus())
        {
            StartShake(Time.deltaTime, SlopeStandPower);
        }
    }

    #region EventInstances

    public void WallJumpShake()
    {
        StartShake(WallJumpLenght, WallJumpPower);

    }

    public void SlopeJumpShake()
    {
        StartShake(SlopeJumpLenght, SlopeJumpPower);
    }

    public void StandSlopeJumpShake()
    {
        StartShake(StandSlopeJumpLenght, StandSlopeJumpPower);
    }

    public void BouncyPlatformShake ()
    {
        StartShake(BouncyPlatformShakeLenght, BouncyPlatformShakePower);
    }

    public void DoorOpenShake()
    {
        StartShake(DoorOpenLenght, DoorOpenPower);

    }

    public void DamageTakingShake()
    {
        StartShake(DamageTakingLenght, DamageTakingPower);

    }

    public void GroundHitDeadShake ()
    {
        StartShake(GroundHitDeadLenght, GroundHitDeadPower);
    }

    public void QTEPressShake ()
    {
        StartShake(QTEPressLenght, QTEPressPower);
    }

    #endregion
}
