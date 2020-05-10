using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public GameObject WallJumpDustParticles;
    public Vector2 WallJumpDustParticlesOffset;
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
        Instantiate(WallJumpDustParticles, player.position + (Vector3)WallJumpDustParticlesOffset, Quaternion.identity);
        print("eaeaezezzezzezz");
    }
}
