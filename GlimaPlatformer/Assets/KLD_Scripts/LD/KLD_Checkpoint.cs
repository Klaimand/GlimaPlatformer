using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Checkpoint : MonoBehaviour
{
    public GameObject[] linkedCheckpoints;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void destroyLinkedCheckpoints ()
    {
        foreach (GameObject _go in linkedCheckpoints)
        {
            Destroy(_go);
        }
        Destroy(gameObject);
    }

}
