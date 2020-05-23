using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_CigaretteSmoke : MonoBehaviour
{
    public LineRenderer line;
    Transform tr;
    Vector3[] positions;
    Vector3[] directions;
    int i = 0;
    float timeSinceUpdate = 0f;
    Material lineMaterial;
    float lineSegment = 0f;
    int currentNumberOfPoints = 2;
    bool allPointsAdded = false;
    
    [SerializeField]
    int numberOfPoints = 10;
    [SerializeField]
    float updateSpeed = 0.25f;
    [SerializeField]
    float riseSpeed = 0.25f;
    [SerializeField]
    float spread = 0.2f;

    [SerializeField, Header("Idle Values")]
    float idleUpdateSpeed = 0.2f;
    [SerializeField]
    float idleRiseSpeed = 0.75f, idleSpread = 0.6f;

    [SerializeField, Header("Run Values")]
    float runUpdateSpeed = 0.1f;
    [SerializeField]
    float runRiseSpeed = 2f, runSpread = 1.6f;
    
    Vector3 tempVec;


    PlayerController2D controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();

        tr = transform;
        line = GetComponent<LineRenderer>();
        lineMaterial = line.material;

        lineSegment = 1f / numberOfPoints;

        positions = new Vector3[numberOfPoints];
        directions = new Vector3[numberOfPoints];

        line.positionCount = numberOfPoints;

        for (int i = 0; i < currentNumberOfPoints; i++)
        {
            tempVec = getSmokeVec();
            directions[i] = tempVec;
            positions[i] = tr.position;
            line.SetPosition(i, positions[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        changeValuesIfRunning();

        timeSinceUpdate += Time.deltaTime;

        if (timeSinceUpdate >= updateSpeed)
        {
            timeSinceUpdate -= updateSpeed;

            if (!allPointsAdded)
            {
                currentNumberOfPoints++;
                line.positionCount = currentNumberOfPoints;
                tempVec = getSmokeVec();
                directions[0] = tempVec;
                positions[0] = tr.position;
                line.SetPosition(0, positions[0]);
            }

            if (!allPointsAdded && (currentNumberOfPoints == numberOfPoints))
            {
                allPointsAdded = true;
            }

            for (int i = currentNumberOfPoints - 1; i > 0; i--)
            {
                tempVec = positions[i - 1];
                positions[i] = tempVec;
                tempVec = directions[i - 1];
                directions[i] = tempVec;
            }
            tempVec = getSmokeVec();
            directions[0] = tempVec;

        }

        for (int i = 1; i < currentNumberOfPoints; i++)
        {
            tempVec = positions[i];
            tempVec += directions[i] * Time.deltaTime;
            positions[i] = tempVec;

            line.SetPosition(i, positions[i]);
        }
        positions[0] = tr.position;
        line.SetPosition(0, tr.position);

        if (allPointsAdded)
        {
            lineMaterial.mainTextureOffset = new Vector2(lineSegment * (timeSinceUpdate / updateSpeed), lineMaterial.mainTextureOffset.y);
        }


    }

    void changeValuesIfRunning ()
    {
        if (controller.playerState == PlayerController2D.PlayerState.Idle ||
            controller.playerState == PlayerController2D.PlayerState.CrouchIdle ||
            controller.playerState == PlayerController2D.PlayerState.Downed ||
            controller.playerState == PlayerController2D.PlayerState.Grabbed)
        {
            updateSpeed = idleUpdateSpeed;
            riseSpeed = idleRiseSpeed;
            spread = idleSpread;
        }
        else
        {
            updateSpeed = runUpdateSpeed;
            riseSpeed = runRiseSpeed;
            spread = runSpread;
        }
    }

    Vector3 getSmokeVec ()
    {
        Vector3 smokeVec = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
        smokeVec.Normalize();
        smokeVec *= spread;
        smokeVec = new Vector3(smokeVec.x, riseSpeed, smokeVec.z);
        return smokeVec;
    }
}
