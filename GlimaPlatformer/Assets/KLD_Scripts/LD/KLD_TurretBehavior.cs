﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class KLD_TurretBehavior : MonoBehaviour
{
    [SerializeField]
    private float maxRotateSpeed = 0f, maxRange = 0f, timeBetweenShoots = 0f, angleToShoot = 0f;

    [SerializeField]
    private bool targetInSight, canShoot, drawSight = false, drawDebugRays = false;
    
    //private Vector3 vectorToTarget;

    public Transform turretIdletarget;
    public Transform playerTarget;
    private Transform target;
    public GameObject bulletObj;
    private LineRenderer lineRenderer;

    private KLD_DamageTaker damagetaker;

    public LayerMask tileMapLayer;


    private Light2D turretLight;

    [SerializeField]
    float onDuration, offDuration;

    bool isLighted;
    float timeSinceLastBlink;

    private void Awake()
    {
        canShoot = true;
        target = turretIdletarget;
        lineRenderer = GetComponent<LineRenderer>();
        turretLight = GetComponent<Light2D>();
    }

    void Start()
    {
        playerTarget = GameObject.Find("Player").transform.GetChild(4);
        damagetaker = GameObject.Find("Player").GetComponent<KLD_DamageTaker>();
    }
    

    void Update()
    {
        doPlayerInSight();
        chooseTarget();
        aimTarget();
        checkIfTurretCanShoot();
        doBlink();
        if (lineRenderer != null)
        {
            drawLaser();
        }
        if (drawDebugRays)
        {
            drawRays();
        }
    }
    

    void chooseTarget ()
    {
        target = targetInSight ? playerTarget : turretIdletarget;
    }


    void aimTarget ()
    {

        Vector3 vectorToTarget = target.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * maxRotateSpeed);

        //pour le fun
        //RaycastHit2D hitt = Physics2D.Raycast((Vector2)transform.position, (Vector2)transform.up, Mathf.Infinity);
        //Debug.DrawRay(transform.position, transform.up * hitt.distance, Color.red);
    }
    

    private void doPlayerInSight ()
    {
        if (Vector3.Distance(transform.position, target.position) <= maxRange && !damagetaker.isInvulnerable)
        {
            Vector3 vectorToPlayer = playerTarget.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, (Vector2)vectorToPlayer, Mathf.Infinity);
            if (drawSight)
            {
                Debug.DrawRay(transform.position, vectorToPlayer * maxRange, new Color(1f, 1f, 0f, 0.2f));
            }
            if (hit && hit.collider.gameObject.CompareTag("Player"))
            {
                targetInSight = true;
            }
            else
            {
                targetInSight = false;
            }
        }
        else
        {
            targetInSight = false;
        }
    }

    private void checkIfTurretCanShoot ()
    {
        if (targetInSight && canShoot && Vector3.Angle(transform.up, target.position - transform.position) <= angleToShoot)
        {
            canShoot = false;
            shoot();
        }
    }

    private void shoot()
    {
        //inst
        Instantiate(bulletObj, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward * 90f));
        StartCoroutine(waitToShoot());
    }

    private IEnumerator waitToShoot ()
    {
        yield return new WaitForSeconds(timeBetweenShoots);
        canShoot = true;
    }

    private void drawRays ()
    {
        //turret sight
        RaycastHit2D hitt = Physics2D.Raycast((Vector2)transform.position, (Vector2)transform.up, Mathf.Infinity);
        Debug.DrawRay(transform.position + transform.up * 0.625f, transform.up * (hitt.distance - 0.625f), Color.red); //celui là

        //Debug.DrawRay(transform.position, (Quaternion.Euler(0f, 0f, angleToShoot) * transform.up).normalized * maxRange, new Color(0f, 1f, 0f, 0.2f));
        //Debug.DrawRay(transform.position, (Quaternion.Euler(0f, 0f, -angleToShoot) * transform.up).normalized * maxRange, new Color(0f, 1f, 0f, 0.2f));
    }

    private void drawLaser ()
    {
        RaycastHit2D hitt = Physics2D.Raycast((Vector2)transform.position, (Vector2)transform.up, Mathf.Infinity, tileMapLayer);
        lineRenderer.SetPosition(0, transform.position + transform.up * 0.625f);
        lineRenderer.SetPosition(1, hitt.point     );
    }

    private void doBlink()
    {
        if (!isLighted && timeSinceLastBlink >= offDuration)
        {
            turretLight.enabled = true;
            timeSinceLastBlink = 0f;
            isLighted = !isLighted;
        }
        else if (isLighted && timeSinceLastBlink >= onDuration)
        {
            turretLight.enabled = false;
            timeSinceLastBlink = 0f;
            isLighted = !isLighted;
        }
        timeSinceLastBlink += Time.deltaTime;
    }

}
