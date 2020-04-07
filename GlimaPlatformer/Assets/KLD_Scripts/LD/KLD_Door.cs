using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Door : MonoBehaviour
{
    [SerializeField]
    float openDistance;
    bool opened;


    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        checkOpen();
    }

    void checkOpen ()
    {
        if (!opened && Vector3.Distance(transform.position, player.position) < openDistance)
        {
            opened = true;
            if (player.position.x < transform.position.x)
            {
                //left
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (player.position.x > transform.position.x)
            {
                //right
                transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
}
