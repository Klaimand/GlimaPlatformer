using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

    #region Variables

    private Rigidbody2D rb;
    private Animator animator;
    private KLD_PlayerEvents events;

    [Header("Movement")]
    public float moveSpeed;
    private float xAxis;
    [SerializeField]
    private float virtualXAxis;
    [SerializeField]
    private float xRawAxis;
    [SerializeField]
    private float virtualXRawAxis;
    public float xAxisSensitivity;
    public int accelerationFrame;
    public int decelerationFrame;
    public bool cantMove;

    private bool cantMoveTrigger;

    [Header("Jump + AirControl")]
    public Vector2 jumpForce; //jump velocity on button press
    public float addAirSpeed;
    public float maxAirSpeed;
    public float horizontalAirDrag;
    public float fallMultiplier; //more means a faster fall
    public float lowJumpMultiplier; //more means a lower minimal jump

    private bool jumped;
    private bool isGrounded; //is the player touching the ground (overlap circle related)
    public float groundDetectionRadius = 0.2f; //radius around ground detection child

    private bool lastJumpIsBounce;

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
    private bool wallJumped;

    [Header("Jump Buffering")]
    public float normalJumpBufferTime;
    private bool isBuffering;
    private bool doneBuffering;

    [Header("GhostJump")]
    public float normalGhostJumpTime;
    private bool canNormalGhostJump; //normal
    private bool doneNormalGhostJump;
    private bool lastFrameGrounded;
    private bool canWallGhostJump; //wall
    public float wallGhostJumpTime;
    private bool doneWallGhostJump;
    private bool lastFrameWallSliding;
    private bool lastFrameAgainstLeftWall;

    [Header("Corner Correction")] //not implemented
    public float cornerDistance;
    //private BoxCollider2D triggerCollider;

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
    public float slopeJumpDrag;
    private bool lastJumpIsSlopeJump;
    private bool isUnderMaxAirSpeedAfterSlopeJump;
    private bool isGoingInTheSlopeDirection;

    [Header("Stairs")]
    public float stairSpeed;
    public float crouchStairsSpeed;
    private bool isOnStairs;
    private bool stairsToTheLeft;
    private bool jumpTrigger;
    
    [Header("Getters and Setters")]
    private bool canTriggerJumpGetter;
    
    [Space(10)]
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public LayerMask whatIsSlidableSlope;
    
    public enum PlayerState {
        Idle, //0
        Running, //1
        CrouchIdle, //2
        CrouchWalk, //3
        Jumping, //4
        Falling, //5
        WallSliding, //6
        TurningBack, //7
        FlatSliding, //8
        SlopeSliding, //9
        SlopeStanding, //10
        CrouchAir, //11
        BlowedAscending, //12
        BlowedFalling, //13
        Downed //14
    };

    [Header("Animations Handling")]
    public PlayerState playerState;
    public float moveStateThreshold;
    private bool flip = false;
    public float rollLenght;
    public float rollSpeed;
    private bool doneRoll;

    #endregion

    #region MonoBehaviour voids

    private void Awake()
    {
        //triggerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        events = GetComponent<KLD_PlayerEvents>();
    }
    
    // Update is called once per frame
    void Update()
    {
        checkGround();
        if (!cantMove)
        {
            doJump();
            doWallJump();
            doCrouch();
            doFlatSliding();
            doSlopedSlideJump();
        }
        getPlayerState2();
        doFlipX();
        checkBlowedGround();
    }

    private void FixedUpdate()
    {
        if (!cantMove)
        {
            manageVirtualRawAxis();
            manageVirtualXAxis();
        }
        else
        {
            virtualXAxis = 0f;
        }
        checkFall();
        doSlopeAndStairsDetection();
        doOnStairsGravityDisable();
        //doSlopeSlideDetection();
        doHorizontalMove();
        checkLastGroundState();
        checkLastWallState();
    }
    
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Tilemap"))
        {
            if (collision.transform.position.x > transform.position.x)
            {

            }
        }
    }*/

    #endregion

    #region Horizontal Movement

    private void manageVirtualRawAxis ()
    {
        if (Input.GetAxisRaw("Horizontal") >= xAxisSensitivity)
        {
            virtualXRawAxis = 1f;
        }
        else if (Input.GetAxisRaw("Horizontal") <= -xAxisSensitivity)
        {
            virtualXRawAxis = -1f;
        }
        else
        {
            virtualXRawAxis = 0f;
        }
    }
    
    private void manageVirtualXAxis ()
    {
        if (virtualXRawAxis == 1f && virtualXAxis < 1f)
        {
            virtualXAxis += 1f / (float)accelerationFrame;
        }
        else if (virtualXRawAxis == -1f && virtualXAxis > -1f)
        {
            virtualXAxis -= 1f / (float)accelerationFrame;
        }
        else if (virtualXRawAxis == 0f)
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
        else if (virtualXRawAxis == 0f && virtualXAxis > -(1f / (float)decelerationFrame) && virtualXAxis < 1f / (float)decelerationFrame)
        {
            virtualXAxis = 0f;
        }
    }

    private void doHorizontalMove () //axis based on ground, velocity and drag based in air
    {
        xAxis = Input.GetAxis("Horizontal");
        xRawAxis = Input.GetAxisRaw("Horizontal");
        if (isGrounded && !isFlatSliding && !isOnStairs && !cantMoveTrigger) //GROUND
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
        else if (!isGrounded && !isAgainstSlidableSlope && !isOnStairs) //AIR
        {
            if (!lastJumpIsSlopeJump) {
                rb.velocity = new Vector2(rb.velocity.x * (1.0f - horizontalAirDrag), rb.velocity.y);
            }
            else if (lastJumpIsSlopeJump)
            {
                rb.velocity = new Vector2(rb.velocity.x * (1.0f - slopeJumpDrag), rb.velocity.y);
            }
            if (!cantControlHorizontal && !cantMove) {
                if ((!lastJumpIsSlopeJump && (Mathf.Abs(rb.velocity.x) < maxAirSpeed)) || lastJumpIsSlopeJump && Mathf.Sign(rb.velocity.x) != Mathf.Sign(Input.GetAxisRaw("Horizontal"))) {
                    rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * addAirSpeed, 0f));
                }
                /*
                if (lastJumpIsSlopeJump)
                {
                    if (Mathf.Abs(rb.velocity.x) < maxAirSpeed)
                    {
                        isUnderMaxAirSpeedAfterSlopeJump = true;
                        lastJumpIsSlopeJump = false;
                    }
                    else
                    {
                        isUnderMaxAirSpeedAfterSlopeJump = false;
                    }
                }*/
                if (!lastJumpIsSlopeJump) {
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
        else if (isOnStairs) //Stairs
        {
            float stairsDirection = stairsToTheLeft ? 1f : -1f;
            float speed = isCrouching ? crouchStairsSpeed : stairSpeed;
            rb.velocity = new Vector2(1f, stairsDirection).normalized * speed * virtualXAxis;
        }
    }

    #endregion

    #region Jumps

    private void doJump ()
    {
        if (Input.GetButtonDown("Fire1") || isBuffering) //classic jump
        {
            if (isGrounded || canNormalGhostJump)
            {
                jumpTrigger = true;
                StartCoroutine(waitToDisableJumpTrigger());
                jumped = true;
                StartCoroutine(applyJumpedOnNextFrame());
                doneBuffering = true;
                doneNormalGhostJump = true;
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.velocity += new Vector2(0, jumpForce.y);
                StartCoroutine(addXVelocityOnNextUpdateAfterJumping());
                events.InvokeJump();
            }
            else if (!isGrounded && !isAgainstSlidableSlope)
            {
                if (!isBuffering)
                {
                    doneBuffering = false;
                    StartCoroutine(startJumpBuffer(normalJumpBufferTime));
                }
            }
        }
    }

    private IEnumerator applyJumpedOnNextFrame ()
    {
        yield return new WaitForFixedUpdate();
        jumped = true;
    }

    private IEnumerator waitToDisableJumpTrigger ()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        jumpTrigger = false;
    }

    private void checkFall ()
    {
        if (rb.velocity.y < 0) //check if we're falling
        {
            if ((isAgainstLeftWall || isAgainstRightWall) && !isCrouching)
            {
                if (rb.velocity.y < 0f) {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (wallSlideMultiplier - 1) * Time.deltaTime; //makes fall slower
                }
            }
            else
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //makes fall faster
            }
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Fire1") && !lastJumpIsWallJump && !lastJumpIsBounce)  //check if we're jumping and gaining height
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
        if (isOnStairs)
        {
            isGrounded = true;
        }
        if (isGrounded)
        {
            jumped = false;
            lastJumpIsWallJump = false;
            lastJumpIsSlopeJump = false;
            lastJumpIsBounce = false;
        }
    }

    private void doWallJump()
    {
        //DETECTION
        //right
        if (Physics2D.OverlapCircle(transform.GetChild(1).transform.position, wallDetectionRadius, whatIsWall) && !isCrouching && !isGrounded) //&& rb.velocity.y < 0)
        {
            isAgainstRightWall = true;
            wallJumped = false;
        }
        else
        {
            isAgainstRightWall = false;
        }
        //left
        if (Physics2D.OverlapCircle(transform.GetChild(2).transform.position, wallDetectionRadius, whatIsWall) && !isCrouching && !isGrounded) //&& rb.velocity.y < 0)
        {
            isAgainstLeftWall = true;
            wallJumped = false;
        }
        else
        {
            isAgainstLeftWall = false;
        }

        //ACTION
        if (((isAgainstLeftWall || isAgainstRightWall) || canWallGhostJump) && (Input.GetButtonDown("Fire1") || isBuffering))
        {
            doneBuffering = true;
            doneWallGhostJump = true;
            lastJumpIsWallJump = true;
            wallJumped = true;
            rb.velocity = Vector2.zero;

            float xVelocitySign = isAgainstLeftWall || lastFrameAgainstLeftWall ? 1f : -1f;
            rb.velocity += new Vector2(wallJumpForce.x * xVelocitySign, wallJumpForce.y);

            cantControlHorizontal = true;
            StartCoroutine(noHorizontalControlDuring(wallJumpNoControlTimer));
            events.InvokeWallJump();
        }
    }

    private IEnumerator applyWallJumpedOnNextFrame()
    {
        yield return new WaitForFixedUpdate();
        wallJumped = true;
    }

    private void doSlopedSlideJump ()
    {
        if (isAgainstSlidableSlope && (Input.GetButtonDown("Fire1") || isBuffering) && !isGrounded)
        {
            doneBuffering = true;
            float velocitySign = isSlidingToTheLeft ? -1f : 1f;

            rb.velocity = new Vector2(rb.velocity.x, SlopeSlideJumpForce.y);

            StartCoroutine(addSlideJumpXVelocity(velocitySign));

            lastJumpIsSlopeJump = true;
            cantControlHorizontal = true;
            StartCoroutine(noHorizontalControlDuring(slopeJumpNoControlTimer));
            if (playerState == PlayerState.SlopeSliding)
            {
                events.InvokeSlopeJump();
            }
            else if (playerState == PlayerState.SlopeStanding)
            {
                events.InvokeStandSlopeJump();
            }
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

    private IEnumerator startJumpBuffer (float bufferTime)
    {
        float curBufferTime = 0f;
        isBuffering = true;
        while (curBufferTime < bufferTime && !doneBuffering) {
            yield return null;
            curBufferTime += Time.deltaTime;
        }
        isBuffering = false;
    }

    private void checkLastGroundState ()
    {
        if (lastFrameGrounded && !isGrounded && !jumped)
        {
            doneNormalGhostJump = false;
            StartCoroutine(startGhostJump());
        }
        lastFrameGrounded = isGrounded;
    }

    private IEnumerator startGhostJump ()
    {
        float curNormalGhostJumpTime = 0f;
        canNormalGhostJump = true;
        while (curNormalGhostJumpTime < normalGhostJumpTime && !doneNormalGhostJump)
        {
            yield return null;
            curNormalGhostJumpTime += Time.deltaTime;
        }
        canNormalGhostJump = false;
    }

    private void checkLastWallState ()
    {
        if (lastFrameWallSliding && (!isAgainstLeftWall && !isAgainstRightWall) && !wallJumped)
        {
            doneWallGhostJump = false;
            StartCoroutine(startWallGhostJump());
        }
        if (!canWallGhostJump)
        {
            lastFrameAgainstLeftWall = isAgainstLeftWall;
        }
        lastFrameWallSliding = isAgainstRightWall || isAgainstLeftWall;
    }

    private IEnumerator startWallGhostJump ()
    {
        float curWallGhostJumpTime = 0f;
        canWallGhostJump = true;
        while (curWallGhostJumpTime < wallGhostJumpTime && !doneWallGhostJump)
        {
            yield return null;
            curWallGhostJumpTime += Time.deltaTime;
        }
        canWallGhostJump = false;
    }

    public void addExplosionForce (Transform mine, float explosionForce)
    {
        cantMoveTrigger = true;
        rb.velocity = new Vector2(transform.position.x - mine.position.x, (transform.position.y - mine.position.y) + 0.8f).normalized * explosionForce;
        StartCoroutine(disableCantMoveTrigger());
    }

    private IEnumerator disableCantMoveTrigger ()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        cantMoveTrigger = false;
    }

    #endregion

    #region Crouching and Sliding

    private void doCrouch ()
    {
        isGoingInTheSlopeDirection = isAgainstSlidableSlope && ((Input.GetAxisRaw("Horizontal") == -1f && isSlidingToTheLeft) || (Input.GetAxisRaw("Horizontal") == 1f && !isSlidingToTheLeft));

        if (Input.GetButtonDown("Fire2") || forceCrouch || isGoingInTheSlopeDirection)
        {
            isCrouching = true;
            coll.size = new Vector2(0.8f, 0.95f);
            coll.offset = new Vector2(0f, 0.475f);
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
            coll.size = new Vector2(0.8f, 1.9f);
            coll.offset = new Vector2(0f, 0.95f);
            spriterenderer.size = new Vector2(1f, 1f);
            //transform.localScale = new Vector3(0.8f, 1.6f, 1f);
        }
    }

    private void doFlatSliding ()
    {
        if (isCrouching && !isOnStairs && ((rb.velocity.x > flatSlidingMinSpeed || rb.velocity.x < -flatSlidingMinSpeed) && isGrounded && !thisFlatSlideHasBeenDone) || isAgainstSlidableSlope)
        {
            if (!isFlatSliding && !isAgainstSlidableSlope) {
                events.InvokeFlatSlide();
            }
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

    private void doSlopeAndStairsDetection ()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position + new Vector3(0f, -0.008f, 0f), -Vector2.up, slopeCastDistance);

        if (hit && 1 << hit.collider.gameObject.layer == whatIsSlidableSlope && !jumpTrigger)
        {
            if (!hit.collider.gameObject.tag.Contains("Stairs"))
            {
                if (!isAgainstSlidableSlope)
                {
                    events.InvokeSlopeSlide();
                }
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
                isOnStairs = true;
                if (hit.collider.gameObject.CompareTag("StairsToTheLeft"))
                {
                    stairsToTheLeft = true;
                }
                else
                {
                    stairsToTheLeft = false;
                }
            }
        }
        else
        {
            isOnStairs = false;
            isAgainstSlidableSlope = false;
        }
    }

    private void doOnStairsGravityDisable ()
    {
        if (isOnStairs && !jumped)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    #endregion

    #region Getters and Setters
    
    public bool getGroundStatus()
    {
        return isGrounded;
    }

    public float getVirtualXAxis ()
    {
        return virtualXAxis;
    }

    public bool getStairsStatus ()
    {
        return isOnStairs;
    }

    public bool getJumpedStatus ()
    {
        if (jumped && canTriggerJumpGetter)
        {
            canTriggerJumpGetter = false;
            return true;
        }
        else if (!jumped)
        {
            canTriggerJumpGetter = true;
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool getSlopeSlideStatus ()
    {
        if (playerState == PlayerState.SlopeSliding)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool getSlopeStandStatus ()
    {
        if (playerState == PlayerState.SlopeStanding)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool getFlatSlideStatus ()
    {
        if (playerState == PlayerState.FlatSliding)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float getFlatSlideSpeedPercentage ()
    {
        return 1 - (flatSlidingMinSpeed / Mathf.Abs(rb.velocity.x));
    }

    public void SetLastJumpIsBounce (bool value)
    {
        lastJumpIsBounce = value;
        StartCoroutine(SetLastJumpIsBounceNextFrames(5, value));
    }

    IEnumerator SetLastJumpIsBounceNextFrames(int nbFrames, bool value)
    {
        float i = 0;
        while (i < nbFrames)
        {
            yield return new WaitForFixedUpdate();
            lastJumpIsBounce = value;
            i++;
        }
    }

    #endregion

    #region Animations Handling

    private void getPlayerState2 ()
    {
        if (!cantMove)
        {
            if (!isCrouching)
            {
                if (!isFlatSliding && !isAgainstSlidableSlope)
                {
                    if (isGrounded)
                    {
                        playerState = Mathf.Abs(rb.velocity.x) > moveStateThreshold ? PlayerState.Running : PlayerState.Idle;
                        if ((Mathf.Sign(rb.velocity.x) != Input.GetAxisRaw("Horizontal")) && Input.GetAxisRaw("Horizontal") != 0f)
                        {
                            //Retournement trigger
                            //print("retourning");
                        }
                    }
                    else if (!isGrounded)
                    {
                        if (!isAgainstRightWall && !isAgainstLeftWall)
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
                else if (isFlatSliding && isAgainstSlidableSlope)
                {
                    playerState = PlayerState.SlopeStanding;
                }
            }
            else if (isCrouching)
            {
                if (!isFlatSliding && !isAgainstSlidableSlope && isGrounded)
                {
                    playerState = Mathf.Abs(rb.velocity.x) > moveStateThreshold ? PlayerState.CrouchWalk : PlayerState.CrouchIdle;
                }
                else if (isFlatSliding)
                {
                    if (!isAgainstSlidableSlope)
                    {
                        playerState = PlayerState.FlatSliding;
                    }
                    else if (isAgainstSlidableSlope)
                    {
                        playerState = PlayerState.SlopeSliding;
                    }
                }
                else if (!isGrounded)
                {
                    playerState = PlayerState.CrouchAir;
                }
            }
        }
        else if (cantMove)
        {
            if (isGrounded)
            {
                playerState = PlayerState.Downed;
            }
            else if (!isGrounded)
            {
                if (rb.velocity.y > 0f)
                {
                    playerState = PlayerState.BlowedAscending;
                }
                else if (rb.velocity.y < 0f)
                {
                    playerState = PlayerState.BlowedFalling;
                }
            }
        }

        animator.SetInteger("PlayerState", (int)playerState);
    }

    private void doFlipX ()
    {
        if (playerState == PlayerState.Running || playerState == PlayerState.Jumping || playerState == PlayerState.Falling ||
            playerState == PlayerState.CrouchWalk || playerState == PlayerState.CrouchAir)
        {
            if (Mathf.Abs(virtualXAxis) > moveStateThreshold && !cantControlHorizontal) {
                flip = Mathf.Sign(virtualXAxis) == -1f;
            }
        }
        else if (isAgainstLeftWall)
        {
            flip = false;
        }
        else if (isAgainstRightWall)
        {
            flip = true;
        }
        if (isAgainstSlidableSlope)
        {
            flip = isSlidingToTheLeft;
        }
        spriterenderer.flipX = flip;
    }


    private void checkBlowedGround ()
    {
        if (cantMove && isGrounded && !doneRoll)
        {
            doneRoll = true;
            StartCoroutine(startRolling());
            events.InvokeGroundHitDead();
        }
        else if (!cantMove)
        {
            doneRoll = false;
        }
    }


    private IEnumerator startRolling ()
    {
        float rollDirection = spriterenderer.flipX ? 1f : -1f;
        float startPos = transform.position.x;
        float endPos = startPos + (rollLenght * rollDirection);

        float time = 0;

        while (transform.position.x != endPos)
        {
            transform.position = new Vector3(Mathf.Lerp(startPos, endPos, time), transform.position.y, transform.position.z);
            time += Time.deltaTime * rollSpeed;
            yield return null;
            if (time > 1f)
            {
                time = 1f;
            }
        }


    }


    #endregion

}
