using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KLD_ScreenShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin virtualCamNoise;
    PlayerController2D controller;

    private float shakeTimeRemaining;
    private float shakePower;
    private float shakeFadeTime;

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

    // Start is called before the first frame update
    void Start()
    {
        virtualCamNoise = virtualCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartShake(1f, 1f);
        }
        checkSlide();
    }

    void LateUpdate()
    {
        doShake();
    }

    void doShake ()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;
            
            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
            //print("Time remaining : " + shakeTimeRemaining + " Power : " + shakePower);
            virtualCamNoise.m_AmplitudeGain = shakePower;
        }
        else
        {
            shakeTimeRemaining = 0f;
            virtualCamNoise.m_AmplitudeGain = 0f;
        }
    }

    private void StartShake(float lenght, float power)
    {
        //print("launched shake lenght : " + lenght + " Power : " + power);
        shakeTimeRemaining = lenght;
        shakePower = power;

        shakeFadeTime = power / lenght;
    }

    void checkSlide()
    {
        if (controller.getFlatSlideStatus()) //&& shakePower < FlatSlidePower)
        {
            print(controller.getFlatSlideSpeedPercentage());
            //StartShake(Time.deltaTime, Mathf.Abs(FlatSlidePower * controller.getFlatSlideSpeedPercentage()));
            StartShake(0.1f, Mathf.Abs(FlatSlidePower * controller.getFlatSlideSpeedPercentage()));

        }
        else if (controller.getSlopeSlideStatus())
        {
            StartShake(0.1f, SlopeSlidePower);
        }
        else if (controller.getSlopeStandStatus())
        {
            StartShake(Time.deltaTime, SlopeStandPower);
        }
    }

    #region EventInstances

    public void DebugJumpShake ()
    {
        StartShake(1f, 10f);
    }

    public void WallJumpShake()
    {
        StartShake(WallJumpLenght, WallJumpPower);

    }

    public void SlopeJumpShake()
    {
        //print("lieb icht");
        StartShake(SlopeJumpLenght, SlopeJumpPower);
    }

    public void StandSlopeJumpShake()
    {
        StartShake(StandSlopeJumpLenght, StandSlopeJumpPower);
    }

    public void BouncyPlatformShake()
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

    public void GroundHitDeadShake()
    {
        StartShake(GroundHitDeadLenght, GroundHitDeadPower);
    }

    public void QTEPressShake()
    {
        StartShake(QTEPressLenght, QTEPressPower);
    }

    #endregion

}
