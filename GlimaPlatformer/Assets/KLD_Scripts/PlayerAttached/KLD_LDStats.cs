using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_LDStats : MonoBehaviour
{
    [SerializeField]
    private int jumps, wallJumps, slopeJumps;
    [SerializeField]
    private float flatSlides, slopeSlides;
    
    public void addJump ()
    {
        jumps++;
    }

    public void addWallJump()
    {
        wallJumps++;
    }

    public void addSlopeJump()
    {
        slopeJumps++;
    }

    public void addFlatSlide()
    {
        flatSlides++;
    }

    public void addSlopeSlide()
    {
        slopeSlides++;
    }
}
