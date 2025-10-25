using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpPower = 12f;
    [SerializeField] float maxJumpTime = 0.2f;
    [SerializeField] float jumpGravityScale = 0.5f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;

    [Header("Flutter Settings")]
    public bool enableFlutter = false;
    public bool canFlutter = true;
    [SerializeField] float flutterMaxTime = 0.45f;
    [SerializeField] float flutterGravityScale = 0.3f;
    [SerializeField] float flutterLift = 3.5f;

    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    [Header("Debug Settings")]
    public static bool debugMenuActive = false;

    float baseGravityScale;
    float jumpTimeCounter;
    float coyoteCounter;
    float jumpBufferCounter;
    bool isGrounded;
    bool isJumping;

    bool isFluttering;
    bool hasFluttered;
    float flutterTimer;

    void Awake()
    {
        baseGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (debugMenuActive) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded)
        {
            hasFluttered = false;
            isFluttering = false;
        }

        coyoteCounter = isGrounded ? coyoteTime : Mathf.Max(0f, coyoteCounter - Time.deltaTime);
        jumpBufferCounter = InputManager.Instance.jumpStarted ? jumpBufferTime : Mathf.Max(0f, jumpBufferCounter - Time.deltaTime);

        Move();
        HandleJump();
        InputManager.Instance.ConsumeFrameInputs();
    }

    void Move()
    {
        Vector2 movement = InputManager.Instance.moveInput;
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
            isFluttering = false;
        }

        if (enableFlutter && !isGrounded && canFlutter && InputManager.Instance.jumpStarted && !hasFluttered && !isJumping)
        {
            isFluttering = true;
            hasFluttered = true;
            flutterTimer = flutterMaxTime;
        }

        if (isJumping && InputManager.Instance.jumpHeld)
        {
            if (jumpTimeCounter > 0f && rb.linearVelocity.y > 0f)
            {
                rb.gravityScale = baseGravityScale * jumpGravityScale;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (InputManager.Instance.jumpCanceled)
        {
            if (isJumping)
            {
                isJumping = false;
                if (rb.linearVelocity.y > 0f) rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }

            isFluttering = false;
        }

        if (enableFlutter && isFluttering && InputManager.Instance.jumpHeld && flutterTimer > 0)
        {
            rb.gravityScale = baseGravityScale * flutterGravityScale;
            if (rb.linearVelocity.y < flutterLift)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, flutterLift);
            }

            flutterTimer -= Time.deltaTime;
        }
        else if (isFluttering)
        {
            isFluttering = false;
        }

        if (rb.linearVelocity.y < 0f && !isFluttering)
        {
            rb.gravityScale = baseGravityScale * fallMultiplier;
        }
        else if (!InputManager.Instance.jumpHeld && !isFluttering)
        {
            rb.gravityScale = baseGravityScale;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
