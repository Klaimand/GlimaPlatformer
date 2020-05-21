using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public GameObject WallJumpDustParticlesLeft;
    public GameObject WallJumpDustParticlesRight;
    public GameObject FallParticle;

    private Transform player;
    PlayerController2D controller;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
    }

    // Update is called once per frame
    private void Update()
    { 

        

    }

    public void CreateWallJumpDustParticles()
    {
        if (controller.getIsWallSlidingLeft())
        {
            Instantiate(WallJumpDustParticlesLeft, player.position, Quaternion.identity);
        }
        else
        {
            Instantiate(WallJumpDustParticlesRight, player.position, Quaternion.identity);
        }
    }

    public void CreateFallParticle ()
    {
        Instantiate(FallParticle, player.position, Quaternion.identity);
    }

}
