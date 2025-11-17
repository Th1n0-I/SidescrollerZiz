using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	private static readonly int IsGrounded  = Animator.StringToHash("isGrounded");
	private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
	private static readonly int IsWalking   = Animator.StringToHash("isWalking");
	private static readonly int Hit         = Animator.StringToHash("PlayerHit");

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
    [SerializeField]  private float                    knockBackIntensity;
    [SerializeField]  private float                    stunTimer;
    [SerializeField]  private GameObject               playerSr;
    [SerializeField]  private GameObject               groundCheck;
	
    [Header("Other")] private Animator                 spriteAnimator;
    private                   Animator                 groundCheckAnimator;
    public                    float                    invulnerabilityTimer = 1;
    public                    HealthBar                healthBar; 
    private                   CinemachineImpulseSource cinemachineImpulseSource;
    private                   AudioManager             audioManager;
    private                   float                    currentJumpHeight;
    [SerializeField] private  float                    maxJumpHeight;
    private                   CapsuleCollider2D        playerCollider;
    public                    bool                     hasDoubleJump;
    private                   bool                     collectedDoubleJump = false;
    private                   List<DoubleJump>         doubleJumps = new List<DoubleJump>();
 
    void Start() {
	    cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        playerRb                 = GetComponent<Rigidbody2D>();
        spriteAnimator           = playerSr.GetComponent<Animator>();
        groundCheckAnimator      = groundCheck.GetComponent<Animator>();
        moveAction               = InputSystem.actions.FindAction("Move");
        jumpAction               = InputSystem.actions.FindAction("Jump");
        crouchAction             = InputSystem.actions.FindAction("Crouch");
        isFacingRight            = true;
	    audioManager             = FindAnyObjectByType<AudioManager>();
	    playerCollider           = GetComponent<CapsuleCollider2D>();


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

    private void OnTriggerExit2D(Collider2D other) {
	    if (other.CompareTag("KillBox")) {
		    KillPlayer();
	    }
    }

    private void KnockBack(Vector3 colliderPosition) {
	    stunned = true;
	    if (colliderPosition.x >= transform.position.x) {
		    playerRb.AddForceAtPosition(new Vector2(-1 * knockBackIntensity, 0), colliderPosition, ForceMode2D.Impulse);
	    }
	    else {
		    playerRb.AddForceAtPosition(new Vector2(1 * knockBackIntensity, 0), colliderPosition, ForceMode2D.Impulse);
	    }
	    Bounce(0.5f, false);
	    StartCoroutine(StunnedTimer(stunTimer));
    }

    IEnumerator StunnedTimer(float timer) {
	    yield return new WaitForSeconds(timer);
	    stunned = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("enemy")  && invulnerabilityTimer <= 0 || other.CompareTag("explosion") && invulnerabilityTimer <= 0)
        {
            healthBar.health -= 1;
            if (healthBar.health <= 0) {
	            KillPlayer();
            }
            audioManager.PlaySound(4);
			KnockBack(other.transform.position);
			cinemachineImpulseSource.GenerateImpulse();
            PlayerHit();
        } else if (other.CompareTag("DoubleJump")) {
	        hasDoubleJump = true;
	        Debug.Log(other);
	        Debug.Log(other.gameObject);
	        Debug.Log(other.GetComponentInParent<DoubleJump>());
	        doubleJumps.Add(other.GetComponentInParent<DoubleJump>());
	        Destroy(other.gameObject);
        } 
    }

    private void KillPlayer() {
	    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlayerHit() {
	    spriteAnimator.SetTrigger("hit");
        invulnerabilityTimer = 1f;
    }

    public void Bounce(float intensity = 1, bool playSound = true) {
        playerRb.linearVelocityY = 0;
        playerRb.AddForce(Vector2.up * jumpForce * intensity, ForceMode2D.Impulse);
        if (playSound) {
	        audioManager.PlaySound(5);
        }
    }
	
    private void ReadPlayerInputs() {
        moveInput = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame() && isGrounded && jumpCooldownTimer <= 0 && !stunned || jumpAction.WasPressedThisFrame() && hasDoubleJump) {
	        playerRb.linearVelocityY = 0;
	        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	        isGrounded        = false;
	        jumpCooldownTimer = jumpCooldownTime;
	        spriteAnimator.SetBool(IsGrounded, false);
	        audioManager.PlaySound(5);
	        hasDoubleJump = false;
        } else if (jumpAction.IsPressed()) {
	        if (currentJumpHeight < maxJumpHeight) {
		        playerRb.AddForce(Vector2.up * (jumpForce * Time.deltaTime * 2), ForceMode2D.Impulse);
		        currentJumpHeight += Time.deltaTime;
	        }
        }
        
        //if (jumpAction.WasPerformedThisFrame() && isGrounded && jumpCooldownTimer <= 0 && !stunned) {
            //playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //isGrounded = false;
            //jumpCooldownTimer = jumpCooldownTime;
            //animator.SetBool(IsGrounded, false);
            //audioManager.PlaySound(5);
        //}

        if (crouchAction.IsPressed()) {
            spriteAnimator.SetBool(IsCrouching, true);
            groundCheckAnimator.SetBool(IsCrouching, true);
            var size = playerCollider.size;
            size.y              = 0.8f;
            playerCollider.size = size;
        } else {
            spriteAnimator.SetBool(IsCrouching, false);
            groundCheckAnimator.SetBool(IsCrouching, false);
            var size = playerCollider.size;
            size.y              = 1.9f;
            playerCollider.size = size;
        }

        if (moveInput.x > 0) {
            isFacingRight = true;
            spriteAnimator.SetBool(IsWalking, true);
        }
        else if (moveInput.x < 0) {
            isFacingRight = false;
            spriteAnimator.SetBool(IsWalking, true);
        }
        else {
            spriteAnimator.SetBool(IsWalking, false);
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
	        hasDoubleJump     = false;
            isGrounded        = true;
            currentJumpHeight = 0;
            spriteAnimator.SetBool(IsGrounded, true);
            doingCoyote = false;
            if (doubleJumps != null) {
	            for (int i = doubleJumps.Count; i > 0; i--) {
		            doubleJumps[i-1].SpawnCollectible();
	            } 
	            doubleJumps.Clear();
            }
            
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
        spriteAnimator.SetBool(IsGrounded, false);
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