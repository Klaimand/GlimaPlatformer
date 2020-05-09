using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_DebugCamera : MonoBehaviour
{
    private Transform cameraTarget;
    private Vector3 startPosition;

    public enum CameraMode {staticc, following, scrolling}
    public CameraMode cameraMode = CameraMode.staticc;
    private Camera cameraComponent;

    public Vector2 CameraOffset;

    public float mouseScrollSensitivity;

    // Start is called before the first frame update
    void Awake()
    {
        startPosition = transform.position;
        cameraComponent = GetComponent<Camera>();
    }

    private void Start()
    {
        cameraTarget = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        //doCameraSize();
    }

    void LateUpdate()
    {
        if (cameraMode == CameraMode.following)
        {
            //cameraLerpToTarget();
        }
        else if (cameraMode == CameraMode.scrolling)
        {
            //cameraLerpToTargetXPositionOnly();
        }
    }

    void cameraLerpToTarget ()
    {
        Vector2 from = transform.position;
        Vector2 target = cameraTarget.position + (Vector3)CameraOffset;

        transform.position = from + (target - from) * (1f - Mathf.Pow(0.01f, Time.deltaTime));
        transform.position += Vector3.forward * -5f;
    }

    void cameraLerpToTargetXPositionOnly ()
    {
        float from = transform.position.x;
        float target = cameraTarget.position.x;

        transform.position = new Vector3(from + (target - from) * (1f - Mathf.Pow(0.01f, Time.deltaTime)), startPosition.y, -10f);
    }

    void doCameraSize ()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            cameraComponent.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * -mouseScrollSensitivity;
        }
    }

}
