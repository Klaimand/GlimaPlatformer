using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KLD_Yacht : MonoBehaviour
{

    public Transform yachtSprites, yachtTransform, floatyYacht;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yachtSprites.position = new Vector3(yachtTransform.position.x, floatyYacht.position.y, 0f);
    }


    public void parentPlayer ()
    {
        player.parent = yachtTransform;
    }

}
