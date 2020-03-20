using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_TurretBullet : MonoBehaviour
{

    [SerializeField]
    float speed;
    
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
