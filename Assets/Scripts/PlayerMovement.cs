using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	private static readonly int IsGrounded  = Animator.StringToHash("isGrounded");
	private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
	private static readonly int IsWalking   = Animator.StringToHash("isWalking");
	
	private Rigidbody2D playerRb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private Vector2 moveInput;

    [Header("PlayerStats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] public bool isFacingRight;

    [Header("GroundCheckRelated")]
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheckPosition;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool doingCoyote;
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpCooldownTime;
    [SerializeField] float jumpCooldownTimer;

    [Header("KnockBackRelated")]
    private bool stunned = false;
    [SerializeField] private float knockBackIntensity;
    [SerializeField] private float stunTimer;
    
    [Header("Other")]
    Animator animator;
    public float     invulnerabilityTimer = 1;
    public HealthBar healthBar; 

 
    void Start() {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        isFacingRight = true;
    }

    

    void Update() {

        invulnerabilityTimer -= Time.deltaTime;
        jumpCooldownTimer -= Time.deltaTime;

        GroundCheck();

        ReadPlayerInputs();
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void KnockBack(Vector3 colliderPosition) {
	    stunned = true;
	    if (colliderPosition.x >= transform.position.x) {
		    playerRb.AddForceAtPosition(new Vector2(-1 * knockBackIntensity, 0), colliderPosition, ForceMode2D.Impulse);
	    }
	    else {
		    playerRb.AddForceAtPosition(new Vector2(1 * knockBackIntensity, 0), colliderPosition, ForceMode2D.Impulse);
	    }
	    Bounce(0.5f);
	    StartCoroutine(StunnedTimer(stunTimer));
    }

    IEnumerator StunnedTimer(float timer) {
	    yield return new WaitForSeconds(timer);
	    stunned = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("enemy") && invulnerabilityTimer <= 0)
        {
            healthBar.health -= 1;
			KnockBack(other.transform.position);
            PlayerHit();

        }
    }

    private void PlayerHit() {
        invulnerabilityTimer = 1f;
    }

    public void Bounce(float intensity = 1) {
        playerRb.linearVelocityY = 0;
        playerRb.AddForce(Vector2.up * jumpForce * intensity, ForceMode2D.Impulse);
    }

    private void ReadPlayerInputs() {
        moveInput = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPerformedThisFrame() && isGrounded && jumpCooldownTimer <= 0 && !stunned) {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            jumpCooldownTimer = jumpCooldownTime;
            animator.SetBool(IsGrounded, false);
        }

        if (crouchAction.IsPressed()) {
            animator.SetBool(IsCrouching, true);
        } else {
            animator.SetBool(IsCrouching, false);
        }

        if (moveInput.x > 0) {
            isFacingRight = true;
            animator.SetBool(IsWalking, true);
        }
        else if (moveInput.x < 0) {
            isFacingRight = false;
            animator.SetBool(IsWalking, true);
        }
        else {
            animator.SetBool(IsWalking, false);
        }
    }

    private void MovePlayer() {
	    if (!stunned) {
		    playerRb.linearVelocityX = moveInput.x * moveSpeed;

		    if (isFacingRight) {
			    transform.rotation = Quaternion.Euler(0, 0, 0);
		    }
		    else {
			    transform.rotation = Quaternion.Euler(0, 180, 0);
		    }
	    }
    }

    private void GroundCheck() {
        Collider2D hit = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundLayer);

        if (hit && jumpCooldownTimer <= 0) {
            isGrounded = true;
            animator.SetBool(IsGrounded, true);
            doingCoyote = false;
        }
        else {
            CoyoteTime();
        }
    }

    private void CoyoteTime() {
        if (!doingCoyote) { 
            StartCoroutine(CoyoteTimer(coyoteTime)); 
            doingCoyote = true;
        }
     
    }

    IEnumerator CoyoteTimer(float timer) {
        yield return new WaitForSeconds(timer);
        isGrounded = false;
        animator.SetBool(IsGrounded, false);
    }

    private void OnDrawGizmos() {
        if (isGrounded) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
    }

}