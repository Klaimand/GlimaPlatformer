using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    private float shakeTimeRemaining;
    private float shakePower;
    private float shakeFadeTime;
    private float shakeRotation;

    public float rotationMultiplier = 15f;
    public float MinimumShake = 0.1f;
    public float MaximumShake = 1f;
    public float MinimumRotation = 1f;
    public float MaximumRotation = 1f;

    public float WallJumpLenght;
    public float WallJumpPower;

    public float SlopeJumpLenght;
    public float SlopeJumpPower;


    void Start()
    {
        
    }
    void Update()

    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartShake( MinimumShake, MaximumShake);
        }
    }
    private void LateUpdate()
    {
        if(shakeTimeRemaining > 0)
        {
            shakeTimeRemaining = Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);
            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
           
        }

        transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation*Random.Range(MinimumRotation, MaximumRotation));
    }
    
    private void StartShake(float lenght, float power)
    {
        shakeTimeRemaining = lenght;
        shakePower = power;

        shakeFadeTime = power / lenght;

        shakeRotation = power * rotationMultiplier;

    }

    public void WallJumpShake()
    {
        StartShake(WallJumpLenght, WallJumpPower);

    }

    public void SlopeJumpShake()
    {
        StartShake(SlopeJumpLenght, SlopeJumpLenght);
       
    }
}
