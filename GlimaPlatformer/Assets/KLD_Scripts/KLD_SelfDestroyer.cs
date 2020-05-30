using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_SelfDestroyer : MonoBehaviour
{
    [SerializeField]
    private float time;
    
    void Start()
    {
        Destroy(gameObject, time);
    }
}
