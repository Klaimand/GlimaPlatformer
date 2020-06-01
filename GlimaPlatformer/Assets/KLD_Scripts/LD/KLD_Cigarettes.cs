using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Cigarettes : MonoBehaviour
{
    public int cigarettesIndex = 0;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void triggerFindAnim ()
    {
        Destroy(gameObject);
    }

}
