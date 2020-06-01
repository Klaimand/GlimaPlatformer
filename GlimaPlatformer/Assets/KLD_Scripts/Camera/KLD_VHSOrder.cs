using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_VHSOrder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().sortingOrder = 800;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
