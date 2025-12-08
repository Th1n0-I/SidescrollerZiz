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

	private static readonly int IsGrounded     = Animator.StringToHash("isGrounded");
	private static readonly int IsCrouching    = Animator.StringToHash("isCrouching");
	private static readonly int IsWalking      = Animator.StringToHash("isWalking");
	private static readonly int Hit            = Animator.StringToHash("hit");
	private static readonly int IsInvulnerable = Animator.StringToHash("isInvulnerable");

	private Rigidbody2D playerRb;
	private InputAction moveAction;
	private InputAction jumpAction;
	private InputAction crouchAction;
	private Vector2     moveInput;

	[Header("PlayerStats")]
	[SerializeField] private float moveSpeed;

	[SerializeField] private float jumpForce;
	[SerializeField] public  bool  isFacingRight;

	[Header("GroundCheckRelated")]
	[SerializeField] bool isGrounded;

	[SerializeField]         Transform groundCheckPosition;
	[SerializeField]         float     groundCheckRadius;
	[SerializeField]         LayerMask groundLayer;
	[SerializeField]         bool      doingCoyote;
	[SerializeField]         float     coyoteTime;
	[SerializeField]         float     jumpCooldownTime;
	[SerializeField]         float     jumpCooldownTimer;
	[SerializeField] private float     maxFallSpeed;

	private float            currentJumpHeight;
	public  bool             hasDoubleJump = false;
	private List<DoubleJump> doubleJumps   = new List<DoubleJump>();
	public  bool             isHoldingJump;
	public  bool             dontDoCoyote;
	public  bool             dontDoCoyoteCooldown;


	[Header("KnockBackRelated")]
	[SerializeField] private float knockBackIntensity;

	[SerializeField] private float                    stunTimer;
	[SerializeField] private GameObject               playerSr;
	[SerializeField] private GameObject               groundCheck;
	private                  bool                     stunned = false;
	public                   float                    invulnerabilityTimer;
	public                   HealthBar                healthBar;
	private                  CinemachineImpulseSource cinemachineImpulseSource;

	[Header("Other")]
	[SerializeField] private float maxJumpHeight;
	[SerializeField] private int               nextLevel;
	private                  AudioManager      audioManager;
	private                  CapsuleCollider2D playerCollider;
	private                  bool              loadingScene = false;
	private                  GameManager       gameManager;


	private void Start() {
		cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
		playerRb                 = GetComponent<Rigidbody2D>();
		spriteAnimator           = playerSr.GetComponent<Animator>();
		groundCheckAnimator      = groundCheck.GetComponent<Animator>();
		moveAction               = InputSystem.actions.FindAction("Move");
		jumpAction               = InputSystem.actions.FindAction("Jump");
		crouchAction             = InputSystem.actions.FindAction("Crouch");
		audioManager             = FindAnyObjectByType<AudioManager>();
		playerCollider           = GetComponent<CapsuleCollider2D>();
		healthBar                = FindAnyObjectByType<HealthBar>();

		isFacingRight = true;

		if (!GameObject.Find("GameManager")) return;
		gameManager = FindAnyObjectByType<GameManager>();
	}

	private void Update() {
		invulnerabilityTimer -= Time.deltaTime;
		jumpCooldownTimer    -= Time.deltaTime;

		GroundCheck();
		ReadPlayerInputs();
		FallCheck();

		if (isHoldingJump && !jumpAction.IsPressed()) {
			isHoldingJump = false;
		}

		if (stunned) return; 
		
		spriteAnimator.SetBool(IsInvulnerable, invulnerabilityTimer > 0);
	}

	private void FallCheck() {
		if (!(playerRb.linearVelocityY <= maxFallSpeed)) return;
		KillPlayer();
	}

	private void FixedUpdate() {
		MovePlayer();
	}

	private void KnockBack(Vector3 colliderPosition) {
		stunned = true;

		playerRb.AddForceAtPosition(colliderPosition.x >= transform.position.x
			                            ? new Vector2(-1 * knockBackIntensity, 0)
			                            : new Vector2(1  * knockBackIntensity, 0), colliderPosition,
		                            ForceMode2D.Impulse);

		Bounce(0.5f, false);
		StartCoroutine(StunnedTimer(stunTimer));
	}

	private IEnumerator StunnedTimer(float timer) {
		yield return new WaitForSeconds(timer);
		stunned = false;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.gameObject.CompareTag("SpikeBlock")) return;
		KillPlayer();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("explosion") && invulnerabilityTimer <= 0) {
			PlayerHit(other);
		} else if (other.CompareTag("enemy") && invulnerabilityTimer <= 0) {
			if (playerRb.linearVelocityY >= -0.1f) {
				PlayerHit(other);
			} else {
				other.GetComponentInParent<Enemy>().HitEnemy();
				Bounce();
			}
		} else if (other.CompareTag("FlyingEnemy") && invulnerabilityTimer <= 0) {
			if (playerRb.linearVelocityY >= -0.1f) {
				PlayerHit(other);
			} else {
				other.GetComponentInParent<FlyingEnemy>().HitEnemy();
				Bounce();
			}
		} else if (other.CompareTag("DoubleJump")) {
			hasDoubleJump = true;
			Debug.Log(other);
			Debug.Log(other.gameObject);
			Debug.Log(other.GetComponentInParent<DoubleJump>());
			doubleJumps.Add(other.GetComponentInParent<DoubleJump>());
			Destroy(other.gameObject);
		} else if (other.CompareTag("Goal")) {
			loadingScene = true;
			SceneManager.LoadScene(nextLevel);
		}
	}

	private void KillPlayer() {
		if (loadingScene) return;
		gameManager.AddDeath();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void PlayerHit(Collider2D other) {
		healthBar.health -= 1;

		KnockBack(other.transform.position);

		cinemachineImpulseSource.GenerateImpulse();
		spriteAnimator.SetTrigger(Hit);
		audioManager.PlaySound(4);

		if (healthBar.health <= 0) {
			KillPlayer();
			return;
		}

		invulnerabilityTimer = 3f;
	}

	public void Bounce(float intensity = 1, bool playSound = true) {
		dontDoCoyote         = true;
		dontDoCoyoteCooldown = true;

		playerRb.linearVelocityY = 0;
		playerRb.AddForce(Vector2.up * jumpForce * intensity, ForceMode2D.Impulse);

		if (playSound) audioManager.PlaySound(5);

		StartCoroutine(CoyoteCooldown());
	}

	private IEnumerator CoyoteCooldown() {
		yield return new WaitForSeconds(0.1f);
		dontDoCoyoteCooldown = false;
	}

	private void Jump(bool normalJump = true) {
		if (normalJump) {
			playerRb.linearVelocityY = 0;
			playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

			isGrounded        = false;
			hasDoubleJump     = false;
			isHoldingJump     = true;
			jumpCooldownTimer = jumpCooldownTime;
			currentJumpHeight = 0;

			spriteAnimator.SetBool(IsGrounded, false);
			audioManager.PlaySound(5);
		} else {
			playerRb.AddForce(Vector2.up * (jumpForce * Time.deltaTime * 10), ForceMode2D.Impulse);
			currentJumpHeight += Time.deltaTime;
		}
	}

	private void ReadPlayerInputs() {
		moveInput = moveAction.ReadValue<Vector2>();

		if ((isGrounded && jumpCooldownTimer <= 0 && !stunned) || hasDoubleJump) {
			if (jumpAction.WasPressedThisFrame()) Jump();
		} else if (isHoldingJump) {
			if (jumpAction.IsPressed()) {
				if (currentJumpHeight < maxJumpHeight) Jump(false);
			}
		}

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

		switch (moveInput.x) {
			case > 0:
				isFacingRight = true;
				spriteAnimator.SetBool(IsWalking, true);
				break;
			case < 0:
				isFacingRight = false;
				spriteAnimator.SetBool(IsWalking, true);
				break;
			default:
				spriteAnimator.SetBool(IsWalking, false);
				break;
		}
	}

	private void MovePlayer() {
		if (stunned) return;
		playerRb.linearVelocityX = moveInput.x * moveSpeed;
		transform.rotation       = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
	}

	private void GroundCheck() {
		var hit = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundLayer);

		if (hit && jumpCooldownTimer <= 0) {
			hasDoubleJump = false;

			currentJumpHeight = 0;
			spriteAnimator.SetBool(IsGrounded, true);
			doingCoyote = false;

			if (!dontDoCoyoteCooldown) {
				dontDoCoyote = false;
				isGrounded   = true;
			} else {
				isGrounded = false;
			}

			if (doubleJumps == null) return;
			for (var i = doubleJumps.Count; i > 0; i--) {
				doubleJumps[i - 1].SpawnCollectible();
			}

			doubleJumps.Clear();
		} else if (!dontDoCoyote) {
			CoyoteTime();
		}
	}

	private void CoyoteTime() {
		if (doingCoyote) return;
		StartCoroutine(CoyoteTimer(coyoteTime));
		doingCoyote = true;
	}

	private IEnumerator CoyoteTimer(float timer) {
		yield return new WaitForSeconds(timer);
		isGrounded = false;
		spriteAnimator.SetBool(IsGrounded, false);
	}

	private void OnDrawGizmos() {
		Gizmos.color = isGrounded ? Color.green : Color.red;
		Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
	}
}