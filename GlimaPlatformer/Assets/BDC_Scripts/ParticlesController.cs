using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public GameObject WallJumpDustParticles;
    public bool IsToTheRight; 

    public float SlopeJumpDustParticles;

    public float SlopeSlideDustParticles;

    public float FlatSlideDustParticles;


    private Transform player;
   

    void Start()
    {
        player = GameObject.Find("Player").transform;
        
    }

    // Update is called once per frame
    private void Update()
    { 

        

    }
    public void CreateWallJumpDustParticles()
    {
        Instantiate(WallJumpDustParticles, player.position, Quaternion.identity);

    }
}
