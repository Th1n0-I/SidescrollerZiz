using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
	// Animations
	private Animator spriteAnimator;
	private Animator groundCheckAnimator;

	private static readonly int IsGrounded  = Animator.StringToHash("isGrounded");
	private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
	private static readonly int IsWalking   = Animator.StringToHash("isWalking");
	private static readonly int Hit         = Animator.StringToHash("PlayerHit");

	private Rigidbody2D playerRb;
	private InputAction moveAction;
	private InputAction jumpAction;
	private InputAction crouchAction;
	private Vector2     moveInput;

	[Header("PlayerStats")] [SerializeField]
	private float moveSpeed;

	[SerializeField] private float jumpForce;
	[SerializeField] public  bool  isFacingRight;

	[Header("GroundCheckRelated")] [SerializeField]
	bool isGrounded;

	[SerializeField]         Transform        groundCheckPosition;
	[SerializeField]         float            groundCheckRadius;
	[SerializeField]         LayerMask        groundLayer;
	[SerializeField]         bool             doingCoyote;
	[SerializeField]         float            coyoteTime;
	[SerializeField]         float            jumpCooldownTime;
	[SerializeField]         float            jumpCooldownTimer;
	private                  float            currentJumpHeight;
	public                   bool             hasDoubleJump = false;
	private                  List<DoubleJump> doubleJumps   = new List<DoubleJump>();
	public                   bool             isHoldingJump;
	public                   bool             dontDoCoyote;
	public                   bool             dontDoCoyoteCooldown;
	[SerializeField] private float            maxFallSpeed;

	[Header("KnockBackRelated")] [SerializeField]
	private float knockBackIntensity;

	[SerializeField] private float                    stunTimer;
	[SerializeField] private GameObject               playerSr;
	[SerializeField] private GameObject               groundCheck;
	private                  bool                     stunned              = false;
	public                   float                    invulnerabilityTimer;
	public                   HealthBar                healthBar;
	private                  CinemachineImpulseSource cinemachineImpulseSource;

	[Header("Other")] [SerializeField] private float             maxJumpHeight;
	[SerializeField]                   private int               nextLevel;
	private                                    AudioManager      audioManager;
	private                                    CapsuleCollider2D playerCollider;
	private                                    bool              loadingScene = false;


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
		healthBar                = FindAnyObjectByType<HealthBar>();
		isHoldingJump            = false;
		dontDoCoyote             = false;
		dontDoCoyoteCooldown     = false;
	}

	void Update() {
		invulnerabilityTimer -= Time.deltaTime;
		jumpCooldownTimer    -= Time.deltaTime;

		GroundCheck();

		ReadPlayerInputs();

		FallCheck();

		if (!stunned && invulnerabilityTimer > 0) {
			playerSr.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
		} else if (invulnerabilityTimer <= 0 && !stunned) {
			playerSr.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		}

		if (isHoldingJump && !jumpAction.IsPressed()) {
			isHoldingJump = false;
		}
	}

	private void FallCheck() {
		Debug.Log(playerRb.linearVelocityY);
		if (playerRb.linearVelocityY <= maxFallSpeed) {
			KillPlayer();
		}
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

		Bounce(0.5f, false);
		StartCoroutine(StunnedTimer(stunTimer));
	}

	IEnumerator StunnedTimer(float timer) {
		yield return new WaitForSeconds(timer);
		stunned = false;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("explosion") && invulnerabilityTimer <= 0) {
			PlayerHit(other);
		}else if (other.CompareTag("enemy") && invulnerabilityTimer <= 0) {
			if (playerRb.linearVelocityY >= -0.1f) {
				PlayerHit(other);
			}
			else {
				other.GetComponentInParent<Enemy>().HitEnemy();
				Bounce();
			}
		}else if (other.CompareTag("FlyingEnemy") && invulnerabilityTimer <= 0) {
			if (playerRb.linearVelocityY >= -0.1f) {
				PlayerHit(other);
			}
			else {
				other.GetComponentInParent<FlyingEnemy>().HitEnemy();
				Bounce();
			}	
		}else if (other.CompareTag("DoubleJump")) {
			hasDoubleJump = true;
			Debug.Log(other);
			Debug.Log(other.gameObject);
			Debug.Log(other.GetComponentInParent<DoubleJump>());
			doubleJumps.Add(other.GetComponentInParent<DoubleJump>());
			Destroy(other.gameObject);
		}
		else if (other.CompareTag("Goal")) {
			loadingScene = true;
			SceneManager.LoadScene(nextLevel);
		}
	}

	private void KillPlayer() {
		if (!loadingScene) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	private void PlayerHit(Collider2D other) {
		healthBar.health -= 1;
		if (healthBar.health <= 0) {
			KillPlayer();
		}
		audioManager.PlaySound(4);
		KnockBack(other.transform.position);
		cinemachineImpulseSource.GenerateImpulse();
		spriteAnimator.SetTrigger("hit");
		invulnerabilityTimer = 3f;
		if (healthBar.health <= 0) {
				KillPlayer();
			}
	}

	public void Bounce(float intensity = 1, bool playSound = true) {
		dontDoCoyote             = true;
		dontDoCoyoteCooldown     = true;
		playerRb.linearVelocityY = 0;
		playerRb.AddForce(Vector2.up * jumpForce * intensity, ForceMode2D.Impulse);
		if (playSound) {
			audioManager.PlaySound(5);
		}

		StartCoroutine(CoyoteCooldown());
	}

	IEnumerator CoyoteCooldown() {
		yield return new WaitForSeconds(0.1f);
		dontDoCoyoteCooldown = false;
	}

	private void Jump(bool normalJump = true) {
		if (normalJump) {
			playerRb.linearVelocityY = 0;
			playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			isGrounded        = false;
			jumpCooldownTimer = jumpCooldownTime;
			spriteAnimator.SetBool(IsGrounded, false);
			audioManager.PlaySound(5);
			hasDoubleJump     = false;
			isHoldingJump     = true;
			currentJumpHeight = 0;
		}
		else {
			playerRb.AddForce(Vector2.up * (jumpForce * Time.deltaTime * 10), ForceMode2D.Impulse);
			currentJumpHeight += Time.deltaTime;
		}
	}
	
	private void ReadPlayerInputs() {
		moveInput = moveAction.ReadValue<Vector2>();

		if (jumpAction.WasPressedThisFrame() && isGrounded && jumpCooldownTimer <= 0 && !stunned ||
		    jumpAction.WasPressedThisFrame() && hasDoubleJump) {
			Jump();
		}
		else if (jumpAction.IsPressed() && isHoldingJump) {
			if (currentJumpHeight < maxJumpHeight) {
				Jump(false);
			} 
		}

		if (crouchAction.IsPressed()) {
			spriteAnimator.SetBool(IsCrouching, true);
			groundCheckAnimator.SetBool(IsCrouching, true);
			var size = playerCollider.size;
			size.y              = 0.8f;
			playerCollider.size = size;
		}
		else {
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
			
			currentJumpHeight = 0;
			spriteAnimator.SetBool(IsGrounded, true);
			doingCoyote  = false;
			if (!dontDoCoyoteCooldown) {
				dontDoCoyote = false;
				isGrounded   = true;
			}
			else {
				isGrounded   = false;
			}
			if (doubleJumps != null) {
				for (int i = doubleJumps.Count; i > 0; i--) {
					doubleJumps[i - 1].SpawnCollectible();
				}

				doubleJumps.Clear();
			}
		}
		else if (!dontDoCoyote) {
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
		}
		else {
			Gizmos.color = Color.red;
		}

		Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
	}
}