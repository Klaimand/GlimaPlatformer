using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

    #region Variables

    private Rigidbody2D rb;
    
    [Header("Movement")]
    public float moveSpeed;
    private float xAxis;
    private float virtualXAxis;
    private float xRawAxis;
    public int accelerationFrame;
    public int decelerationFrame;
    //public float xAxisStopThreshold;

    [Header("Jump + AirControl")]
    public Vector2 jumpForce; //jump velocity on button press
    public float addAirSpeed;
    public float maxAirSpeed;
    public float horizontalAirDrag;
    public float fallMultiplier; //more means a faster fall
    public float lowJumpMultiplier; //more means a lower minimal jump

    private bool isGrounded; //is the player touching the ground (overlap circle related)
    public float groundDetectionRadius = 0.2f; //radius around ground detection child

    [Header("WallJump")]
    public float wallSlideMultiplier;
    public float wallDetectionRadius = 0.05f;
    private bool isAgainstRightWall;
    private bool isAgainstLeftWall;
    public Vector2 wallJumpForce;
    public float wallJumpNoControlTimer;
    private float wallJumpNoControlCurrentTime;
    private bool cantControlHorizontal;
    private bool lastJumpIsWallJump;

    [Header("Crouch + FlatSlide")]
    public float crouchSpeed;
    private bool isCrouching;
    private CapsuleCollider2D coll;
    private SpriteRenderer spriterenderer;
    public float flatSlideDrag;
    private bool isFlatSliding;
    public float flatSlidingMinSpeed;
    private bool thisFlatSlideHasBeenDone; //become true when the player finish a flat slide, and become false again when the player is not crouching
    public bool forceCrouch;

    [Header("SlopeSlide")]
    public float slideCrouchingXSpeedMultiplier;
    public float slideCrouchingMaxSpeed;
    public float slideStandingDrag;
    public float slideStandingMaxSpeed;
    private bool isAgainstSlidableSlope;
    public float slopeEndDetectionRayLenght;
    //private bool isNearSlopeEnd;
    public float slopeCastDistance;
    public Vector2 SlopeSlideJumpForce;
    

    [Space(10)]
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public LayerMask whatIsSlidableSlope;
    
    public enum PlayerState {Idle, Running, CrouchIdle, CrouchWalk, Jumping, Falling, WallSliding}; //retournement, flatslide, slopeslide, slopestand
    [Header("Animations Handling")]
    public PlayerState playerState;
    public float moveStateThreshold; //Horizontal Axis related (can't be 0)

    #endregion

    #region MonoBehaviour voids

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        doJump();
        checkGround();
        doWallJump();
        doCrouch();
        doFlatSliding();
        doSlopedSlideJump();
        getPlayerState();
    }

    private void FixedUpdate()
    {
        manageVirtualXAxis();
        doSlopeSlideDetection();
        doHorizontalMove();
    }

    #endregion

    #region Horizontal Movement

    private void manageVirtualXAxis ()
    {
        if (Input.GetAxisRaw("Horizontal") == 1f && virtualXAxis < 1f)
        {
            virtualXAxis += 1f / (float)accelerationFrame;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1f && virtualXAxis > -1f)
        {
            virtualXAxis -= 1f / (float)accelerationFrame;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0f)
        {
            if (virtualXAxis > 0f)
            {
                virtualXAxis -= 1f / (float)decelerationFrame;
            }
            if (virtualXAxis < 0f)
            {
                virtualXAxis += 1f / (float)decelerationFrame;
            }
        }

        if (virtualXAxis > 1f)
        {
            virtualXAxis = 1f;
        }
        else if (virtualXAxis < -1f)
        {
            virtualXAxis = -1f;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0f && virtualXAxis > -(1f / (float)decelerationFrame) && virtualXAxis < 1f / (float)decelerationFrame)
        {
            virtualXAxis = 0f;
        }
    }

    private void doHorizontalMove () //axis based on ground, velocity and drag based in air
    {
        xAxis = Input.GetAxis("Horizontal");
        xRawAxis = Input.GetAxisRaw("Horizontal");
        if (isGrounded && !isFlatSliding)
        {
            if (!cantControlHorizontal && !isCrouching)
            {
                rb.velocity = new Vector2(virtualXAxis * moveSpeed, rb.velocity.y);
            }
            else if (!cantControlHorizontal && isCrouching)
            {
                rb.velocity = new Vector2(virtualXAxis * crouchSpeed, rb.velocity.y);
            }
        }
        else if (!isGrounded && !cantControlHorizontal && !isAgainstSlidableSlope)
        {
            rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * addAirSpeed, 0f));
            rb.velocity = new Vector2(rb.velocity.x * (1.0f - horizontalAirDrag), rb.velocity.y);
            if (rb.velocity.x > maxAirSpeed)
            {
                rb.velocity = new Vector2(maxAirSpeed, rb.velocity.y);
            }
            else if (rb.velocity.x < -maxAirSpeed)
            {
                rb.velocity = new Vector2(-maxAirSpeed, rb.velocity.y);
            }
        }
        else if (isAgainstSlidableSlope)
        {
            if (isCrouching)
            {
                rb.velocity = new Vector2(rb.velocity.x * slideCrouchingXSpeedMultiplier, rb.velocity.y);
                if (rb.velocity.x > slideCrouchingMaxSpeed)
                {
                    rb.velocity = new Vector2(slideCrouchingMaxSpeed, rb.velocity.y);
                }
            }
            else if (!isCrouching)
            {
                rb.velocity = new Vector2(rb.velocity.x * (1.0f - slideStandingDrag), rb.velocity.y);
                if (rb.velocity.x > slideStandingMaxSpeed)
                {
                    rb.velocity = new Vector2(slideStandingMaxSpeed, rb.velocity.y);
                }
            }
        }
        else if (isFlatSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x * (1.0f - flatSlideDrag), rb.velocity.y);
        }
    }

    #endregion

    #region Jumps

    private void doJump ()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) //check if we can jump
        {
            rb.velocity += new Vector2(0, jumpForce.y); //jump
            StartCoroutine(addXVelocityOnNextUpdateAfterJumping());
        }
        if (rb.velocity.y < 0) //check if we're falling
        {
            if ((isAgainstLeftWall || isAgainstRightWall) && !isCrouching)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (wallSlideMultiplier - 1) * Time.deltaTime; //makes fall faster
            }
            else
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //makes fall faster
            }
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.UpArrow) && !lastJumpIsWallJump) //check if we're jumping and gaining height
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; //make gravity less
        }
    }

    private IEnumerator addXVelocityOnNextUpdateAfterJumping ()
    {
        yield return new WaitForFixedUpdate();
        rb.velocity += new Vector2(jumpForce.x * virtualXAxis, 0); //jump
    }

    void checkGround()
    {
        isGrounded = Physics2D.OverlapCircle(transform.GetChild(0).transform.position, groundDetectionRadius, whatIsGround);
        if (isGrounded)
        {
            lastJumpIsWallJump = false;
        }
    }

    private void doWallJump()
    {
        //DETECTION
        //right
        if (Physics2D.OverlapCircle(transform.GetChild(1).transform.position, wallDetectionRadius, whatIsWall) && !isCrouching && !isGrounded && rb.velocity.y < 0) //&& Input.GetAxisRaw("Horizontal") > 0.1f)
        {
            isAgainstRightWall = true;
        }
        else
        {
            isAgainstRightWall = false;
        }
        //left
        if (Physics2D.OverlapCircle(transform.GetChild(2).transform.position, wallDetectionRadius, whatIsWall) && !isCrouching && !isGrounded && rb.velocity.y < 0) //&& Input.GetAxisRaw("Horizontal") < -0.1f)
        {
            isAgainstLeftWall = true;
        }
        else
        {
            isAgainstLeftWall = false;
        }

       //ACTION
        if (isAgainstRightWall && Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastJumpIsWallJump = true;
            rb.velocity = new Vector2(0,0);
            rb.velocity += new Vector2(-wallJumpForce.x, wallJumpForce.y);
            cantControlHorizontal = true;
            StartCoroutine(wallJumpNoControl());
        }
        else if (isAgainstLeftWall && Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastJumpIsWallJump = true;
            rb.velocity = new Vector2(0,0);
            rb.velocity += new Vector2(wallJumpForce.x, wallJumpForce.y);
            cantControlHorizontal = true;
            StartCoroutine(wallJumpNoControl());
        }
    }

    private IEnumerator wallJumpNoControl ()
    {
        yield return new WaitForSeconds(wallJumpNoControlTimer);
        cantControlHorizontal = false;
    }

    private void doSlopedSlideJump () //wip
    {
        if (isAgainstSlidableSlope && Input.GetKeyDown(KeyCode.UpArrow) && !isGrounded)
        {
            float velocitySign = Mathf.Sign(rb.velocity.x);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.velocity += new Vector2(velocitySign * SlopeSlideJumpForce.x, SlopeSlideJumpForce.y);
            /*
            float curSpeedPercentage = rb.velocity.x / slideCrouchingMaxSpeed;

            rb.velocity += new Vector2(-Mathf.Sign(rb.velocity.x) * curSpeedPercentage * SlopeSlideJumpForce.x, SlopeSlideJumpForce.y);
            */
        }
    }

    #endregion

    #region Crouching and Sliding

    private void doCrouch ()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || forceCrouch)
        {
            isCrouching = true;
            coll.size = new Vector2(1f, 0.5f);
            coll.offset = new Vector2(0f, 0.25f);
            spriterenderer.size = new Vector2(1f, 0.5f);
            //transform.localScale = new Vector3(0.8f,0.8f,1f);
        }

        bool isUnderCollider = false;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.GetChild(3).position, new Vector2(0.4f, 0.7f), 0f, Vector2.up, 0.4f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject != gameObject)
            {
                isUnderCollider = true;
            }
        }
        if (!isUnderCollider && !Input.GetKey(KeyCode.DownArrow) && isCrouching && !forceCrouch)
        {
            isCrouching = false;
            coll.size = new Vector2(1f, 1f);
            coll.offset = new Vector2(0f, 0.5f);
            spriterenderer.size = new Vector2(1f, 1f);
            //transform.localScale = new Vector3(0.8f, 1.6f, 1f);
        }
    }

    private void doFlatSliding ()
    {
        if (isCrouching && ((rb.velocity.x > flatSlidingMinSpeed || rb.velocity.x < -flatSlidingMinSpeed) && isGrounded && !thisFlatSlideHasBeenDone) || isAgainstSlidableSlope)
        {
            isFlatSliding = true;
        }
        else
        {
            isFlatSliding = false;
            thisFlatSlideHasBeenDone = true;
        }
        if (!isCrouching || isAgainstSlidableSlope)
        {
            thisFlatSlideHasBeenDone = false;
        }
    }
    
    private void doSlopeSlideDetection ()
    {
        Debug.DrawRay(transform.GetChild(0).position + new Vector3(0f, -0.008f, 0f), -Vector2.up * slopeCastDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position + new Vector3(0f,-0.008f, 0f), -Vector2.up, slopeCastDistance, whatIsSlidableSlope);

        if (hit == true)
        {
            isAgainstSlidableSlope = true;
        }
        else
        {
            isAgainstSlidableSlope = false;
        }
    }

    #endregion

    #region Animations Handling

    private void getPlayerState () //obsolete
    {
        if (!isCrouching)
        {
            if (isGrounded)
            {
                if (xAxis >= moveStateThreshold || xAxis <= -moveStateThreshold)
                {
                    playerState = PlayerState.Running;
                }
                else if (xAxis < moveStateThreshold && xAxis > -moveStateThreshold)
                {
                    playerState = PlayerState.Idle;
                }
            }
            else if (!isGrounded)
            {
                if (!isAgainstLeftWall && !isAgainstRightWall)
                {
                    if (rb.velocity.y > 0f)
                    {
                        playerState = PlayerState.Jumping;
                    }
                    else if (rb.velocity.y < 0f)
                    {
                        playerState = PlayerState.Falling;
                    }
                }
                else if (isAgainstLeftWall || isAgainstRightWall)
                {
                    playerState = PlayerState.WallSliding;
                }
            }
        }
        else if (isCrouching)
        {
            if (xAxis >= moveStateThreshold || xAxis <= -moveStateThreshold)
            {
                playerState = PlayerState.CrouchWalk;
            }
            else if (xAxis < moveStateThreshold && xAxis > -moveStateThreshold)
            {
                playerState = PlayerState.CrouchIdle;
            }
        }
    }

    #endregion

}
