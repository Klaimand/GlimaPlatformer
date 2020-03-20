using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_TurretBehavior : MonoBehaviour
{
    [SerializeField]
    private float maxRotateSpeed, maxRange, timeBetweenShoots, angleToShoot;

    [SerializeField]
    private bool targetInSight, canShoot;
    
    //private Vector3 vectorToTarget;

    public Transform turretIdletarget;
    public Transform playerTarget;
    private Transform target;
    public GameObject bulletObj;


    void Start()
    {
        canShoot = true;
        target = turretIdletarget;
    }
    

    void Update()
    {
        doPlayerInSight();
        chooseTarget();
        aimTarget();
        checkIfTurretCanShoot();
        drawRays();
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
        if (Vector3.Distance(transform.position, target.position) <= maxRange)
        {
            Vector3 vectorToPlayer = playerTarget.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, (Vector2)vectorToPlayer, Mathf.Infinity);
            Debug.DrawRay(transform.position, vectorToPlayer * maxRange, new Color(1f, 1f, 0f, 0.2f));
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
        Debug.DrawRay(transform.position, transform.up * hitt.distance, Color.red);

        Debug.DrawRay(transform.position, (Quaternion.Euler(0f, 0f, angleToShoot) * transform.up).normalized * maxRange, new Color(0f, 1f, 0f, 0.2f));
        Debug.DrawRay(transform.position, (Quaternion.Euler(0f, 0f, -angleToShoot) * transform.up).normalized * maxRange, new Color(0f, 1f, 0f, 0.2f));
    }

}
