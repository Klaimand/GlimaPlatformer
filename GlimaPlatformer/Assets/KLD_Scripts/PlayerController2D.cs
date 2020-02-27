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
    private bool thisFlatSlideHasBeenDone; //become true when the player finish a flat slide, and become false again when the player is not crouching (more complex now)
    public bool forceCrouch;

    [Header("SlopeSlide")]
    public float slideCrouchingXSpeedMultiplier;
    public float slideCrouchingMaxSpeed;
    public float slideStandingDrag;
    public float slideStandingMaxSpeed;
    public float slopeJumpNoControlTimer;
    private bool isAgainstSlidableSlope;
    private bool isSlidingToTheLeft;
    public float slopeCastDistance;
    public Vector2 SlopeSlideJumpForce;
    private bool lastJumpIsSlopeJump;
    private bool isGoingInTheSlopeDirection;
    

    [Space(10)]
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public LayerMask whatIsSlidableSlope;
    
    public enum PlayerState {
        Idle,
        Running,
        CrouchIdle,
        CrouchWalk, //Marcher accroupi
        Jumping, //Dans les airs quand on prend de la hauteur
        Falling, //Dans les airs quand on perd de la hauteur
        WallSliding, //Frotte contre un mur, peut potentiellement walljump, tombe beaucoup moins vite que s'il ne frottait pas
        TurningBack, //Change de direction brusquement en courant au sol
        FlatSliding, //Glissade sur un sol plat après avoir couru
        SlopeSliding, //Glissade sur un sol penché a 45°, va assez vite
        SlopeStanding //Essaye de tenir debout sur un sol penché a 45°, glisse assez doucement
    };

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
        checkFall();
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
        if (isGrounded && !isFlatSliding) //GROUND
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
        else if (!isGrounded && !isAgainstSlidableSlope) //AIR
        {
            rb.velocity = new Vector2(rb.velocity.x * (1.0f - horizontalAirDrag), rb.velocity.y);
            if (!cantControlHorizontal) {
                rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * addAirSpeed, 0f));
                if (rb.velocity.x > maxAirSpeed)
                {
                    rb.velocity = new Vector2(maxAirSpeed, rb.velocity.y);
                }
                else if (rb.velocity.x < -maxAirSpeed)
                {
                    rb.velocity = new Vector2(-maxAirSpeed, rb.velocity.y);
                }
            }
        }
        else if (isAgainstSlidableSlope) //SLOPE SLIDE
        {
            if (isCrouching) //CROUCHED
            {
                rb.velocity = new Vector2(rb.velocity.x * slideCrouchingXSpeedMultiplier, rb.velocity.y);
                if (rb.velocity.x > slideCrouchingMaxSpeed || rb.velocity.x < -slideCrouchingMaxSpeed)
                {
                    float velocitySign = Mathf.Sign(rb.velocity.x);
                    rb.velocity = new Vector2(slideCrouchingMaxSpeed * velocitySign, rb.velocity.y);
                }
            }
            else if (!isCrouching) //STANDING
            {
                rb.velocity = new Vector2(rb.velocity.x * (1.0f - slideStandingDrag), rb.velocity.y);
                if (rb.velocity.x > slideStandingMaxSpeed || rb.velocity.x < -slideStandingMaxSpeed)
                {
                    float velocitySign = Mathf.Sign(rb.velocity.x);
                    rb.velocity = new Vector2(slideStandingMaxSpeed * velocitySign, rb.velocity.y);
                }
            }
        }
        else if (isFlatSliding) //FLATSLIDING
        {
            rb.velocity = new Vector2(rb.velocity.x * (1.0f - flatSlideDrag), rb.velocity.y);
        }
    }

    #endregion

    #region Jumps

    private void doJump ()
    {
        if (Input.GetButtonDown("Fire1") && isGrounded) //classic jump
        {
            rb.velocity += new Vector2(0, jumpForce.y);
            StartCoroutine(addXVelocityOnNextUpdateAfterJumping());
        }
    }

    private void checkFall ()
    {
        if (rb.velocity.y < 0) //check if we're falling
        {
            if ((isAgainstLeftWall || isAgainstRightWall) && !isCrouching)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (wallSlideMultiplier - 1) * Time.deltaTime; //makes fall slower
            }
            else
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //makes fall faster
            }
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Fire1") && !lastJumpIsWallJump)  //check if we're jumping and gaining height
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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
            lastJumpIsSlopeJump = false;
        }
    }

    private void doWallJump()
    {
        //DETECTION
        //right
        if (Physics2D.OverlapCircle(transform.GetChild(1).transform.position, wallDetectionRadius, whatIsWall) && !isCrouching && !isGrounded && rb.velocity.y < 0)
        {
            isAgainstRightWall = true;
        }
        else
        {
            isAgainstRightWall = false;
        }
        //left
        if (Physics2D.OverlapCircle(transform.GetChild(2).transform.position, wallDetectionRadius, whatIsWall) && !isCrouching && !isGrounded && rb.velocity.y < 0)
        {
            isAgainstLeftWall = true;
        }
        else
        {
            isAgainstLeftWall = false;
        }

        //ACTION
        if ((isAgainstLeftWall || isAgainstRightWall) && Input.GetButtonDown("Fire1"))
        {
            lastJumpIsWallJump = true;
            rb.velocity = Vector2.zero;

            float xVelocitySign = isAgainstLeftWall ? 1f : -1f;
            rb.velocity += new Vector2(wallJumpForce.x * xVelocitySign, wallJumpForce.y);

            cantControlHorizontal = true;
            StartCoroutine(noHorizontalControlDuring(wallJumpNoControlTimer));
        }
    }
    
    private void doSlopedSlideJump ()
    {
        if (isAgainstSlidableSlope && Input.GetButtonDown("Fire1") && !isGrounded)
        {
            float velocitySign = isSlidingToTheLeft ? -1f : 1f;

            rb.velocity = new Vector2(rb.velocity.x, SlopeSlideJumpForce.y);

            StartCoroutine(addSlideJumpXVelocity(velocitySign));

            cantControlHorizontal = true;
            StartCoroutine(noHorizontalControlDuring(slopeJumpNoControlTimer));
        }
    }

    private IEnumerator addSlideJumpXVelocity (float velocitySign)
    {
        yield return new WaitForFixedUpdate();
        rb.velocity += new Vector2(velocitySign * SlopeSlideJumpForce.x, 0f);
    }

    private IEnumerator noHorizontalControlDuring(float noControlTime)
    {
        yield return new WaitForSeconds(noControlTime);
        cantControlHorizontal = false;
    }

    #endregion

    #region Crouching and Sliding

    private void doCrouch ()
    {
        isGoingInTheSlopeDirection = isAgainstSlidableSlope && ((Input.GetAxisRaw("Horizontal") == -1f && isSlidingToTheLeft) || (Input.GetAxisRaw("Horizontal") == 1f && !isSlidingToTheLeft));

        if (Input.GetButtonDown("Fire2") || forceCrouch || isGoingInTheSlopeDirection)
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
        if ((!isUnderCollider && !Input.GetButton("Fire2") && isCrouching) && !forceCrouch && !isGoingInTheSlopeDirection)
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
        if ((!isCrouching || isAgainstSlidableSlope) || (isCrouching && !isGrounded && !isAgainstSlidableSlope))
        {
            thisFlatSlideHasBeenDone = false;
        }
    }
    
    private void doSlopeSlideDetection ()
    {
        //Debug.DrawRay(transform.GetChild(0).position + new Vector3(0f, -0.008f, 0f), -Vector2.up * slopeCastDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position + new Vector3(0f,-0.008f, 0f), -Vector2.up, slopeCastDistance, whatIsSlidableSlope);

        if (hit == true)
        {
            isAgainstSlidableSlope = true;
            if (hit.collider.gameObject.CompareTag("SlopeToTheLeft"))
            {
                isSlidingToTheLeft = true;
            }
            else
            {
                isSlidingToTheLeft = false;
            }
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
