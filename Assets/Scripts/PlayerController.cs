using System;
using System.Collections;
using UnityEngine;

public class PlayerController : DontDestroy
{
    //private BoxCollider2D boxColl;
    private CapsuleCollider2D capsuleColl;
    private Rigidbody2D rigidBody;
    private Animator animator;

    [SerializeField] private AudioManager audioManager;
    private int gravityScaleNormal = 5;
    private int gravityScaleUnder = -5;

    //
    private bool isUnder;
    private bool isGrounded;
    private bool isDoubleJump;
    private bool canDoubleJump;
    private bool isRolling;
    private float yNormal = -1.725f;
    public float yUnder = -5.265f;
    [SerializeField] private float jumpForce = 16;
    public float baseSpeed = 10f;
    public float speed = 10f;
    //[SerializeField] private Vector2 boxOffset;
    private float velocityThreshold = 0.01f;

    //ground check raycast
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    void Awake()
    {
        //boxColl = GetComponent<BoxCollider2D>();
        capsuleColl = GetComponent<CapsuleCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();

        //boxOffset = boxColl.offset;

        isUnder = false;
        isRolling = false;
        isGrounded = false;
        canDoubleJump = false;
        isDoubleJump = false;
        animator.SetFloat("speed", baseSpeed);
    }

    void Update()
    {
        CheckGroundStatus();
        HandleMoveInput();
        UpdateAnimator();
        UpdateCollision();
    }
    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        // Update animator and movement states when landing
        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
            animator.SetFloat("verticalSpeed", 0);
            speed = baseSpeed;
            canDoubleJump = true;
            isDoubleJump = false;
            isRolling = false;
        }
        // Update animator when leaving ground
        else
        {
            animator.SetBool("isGrounded", false);
            speed = 0;
        }

    }
    public void HandleMoveInput()
    {
        // Xử lý khi nhấn phím UpArrow
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleButtonUp();
        }

        // Xử lý nhấn phím DownArrow
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleButtonDown();
        }
    }

    public void FlipY(bool isFlip = true)
    {
        Vector3 currentScale = transform.localScale;
        float scaleY = Math.Abs(currentScale.y);
        transform.localScale = new Vector3(currentScale.x, isFlip ? -scaleY : scaleY, currentScale.z);
    }
    public void HandleButtonUp()
    {
        if (isUnder)
        {
            isRolling = true; // Kích hoạt animation xoay
            rigidBody.bodyType = RigidbodyType2D.Kinematic; // Đặt thành Kinematic để xuyên qua
            StartCoroutine(MoveToNormal()); // Di chuyển và xoay lên trên
        }
        else if (isGrounded)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            isGrounded = false; // Đặt lại isGrounded khi nhảy
            canDoubleJump = true;
            isDoubleJump = false;
            speed = 0;

            audioManager.PlaySfxJump();
        }
        else if (canDoubleJump)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce * 0.9f);
            isRolling = true;
            canDoubleJump = false; // Vô hiệu hóa nhảy kép sau khi sử dụng
            isDoubleJump = true;
            speed = 0;

            audioManager.PlaySfxJump();
        }
    }
    public void HandleButtonDown()
    {
        if (!isGrounded || isUnder) return;
        speed = 0;
        isRolling = true; // Kích hoạt animation xoay
        rigidBody.bodyType = RigidbodyType2D.Kinematic; // Đặt thành Kinematic để xuyên qua
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 0);
        StartCoroutine(MoveUnderGround());
    }

    private IEnumerator MoveUnderGround()
    {
        float duration = 0.25f; // Thời gian di chuyển và animation
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new(transform.position.x, yUnder, transform.position.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        transform.position = targetPos;
        FlipY(true);
        rigidBody.gravityScale = gravityScaleUnder;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        isRolling = false; // Kết thúc animation xoay
        isUnder = true;
    }

    private IEnumerator MoveToNormal()
    {
        float duration = 0.25f; // Thời gian di chuyển và animation
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new(transform.position.x, yNormal, transform.position.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        transform.position = targetPos;
        FlipY(false); // Đặt lại scale để nhân vật hướng lên
        rigidBody.gravityScale = gravityScaleNormal;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        isRolling = false; // Kết thúc animation xoay
        isUnder = false;
    }

    void UpdateAnimator()
    {
        float verticalSpeed = rigidBody.linearVelocity.y;
        if (Mathf.Abs(verticalSpeed) < velocityThreshold)
        {
            verticalSpeed = 0f;
        }
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("verticalSpeed", verticalSpeed);
        animator.SetBool("isDoubleJump", isDoubleJump);
        animator.SetBool("isRolling", isRolling);
        animator.SetFloat("speed", speed);
    }
    void UpdateCollision()
    {
        // if (IsAnimationPlaying("roll"))
        // {
        //     boxColl.offset = new Vector2(boxOffset.x, 0);
        // }
        // else if (IsAnimationPlaying("jump") || IsAnimationPlaying("fall"))
        // {
        //     boxColl.offset = new Vector2(boxOffset.x, -0.4f);
        // }
        // else
        // {
        //     boxColl.offset = boxOffset;
        // }
        if (IsAnimationPlaying("roll"))
        {
            capsuleColl.size = new Vector2(0.68f, 0.8f);
        }
        else
        {
            capsuleColl.size = new Vector2(0.68f, 1.6f);
        }
    }
    bool IsAnimationPlaying(string animationName)
    {
        // Lấy trạng thái hiện tại của Animator (layer 0)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // Kiểm tra nếu Animation Clip có tên khớp với animationName
        return stateInfo.IsName(animationName);
    }
    public override void ResetState()
    {
        transform.position = new Vector2(transform.position.x, yNormal);
        FlipY(false);
        rigidBody.gravityScale = gravityScaleNormal;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        isRolling = false;
        isUnder = false;
        Debug.Log("Reset state - player");
    }
}
