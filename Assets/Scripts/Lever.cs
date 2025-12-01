using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lever : MonoBehaviour {
	private static readonly int            IsFlipped = Animator.StringToHash("isFlipped");
	public                  bool           flipped   = false;
	private                 Animator       animator;
	public                  bool           playerInRange;
	private                 InputAction    interact;
	private                 SpriteRenderer keycapSr;
	private                 GameObject     player;
	public                  Platform       platformScript;

	[SerializeField] private GameObject platform;
	[SerializeField] private GameObject keycap;

	private void Start() {
		animator       = gameObject.GetComponent<Animator>();
		interact       = InputSystem.actions.FindAction("Interact");
		keycapSr       = keycap.GetComponent<SpriteRenderer>();
		platformScript = platform.GetComponent<Platform>();
		player         = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update() {
		CheckPlayerInputs();
		KeycapOpacity();
	}

	private void KeycapOpacity() {
		var color = keycapSr.color;
		color.a        = 1.5f - Vector2.Distance(player.transform.position, keycap.transform.position) / 3;
		keycapSr.color = color;
	}

	private void CheckPlayerInputs() {
		if (interact.WasPerformedThisFrame() && playerInRange) {
			FlipLever();
		}
	}

	public void FlipLever() {
		if (!flipped) {
			FlipLeverOn();
		}
		else {
			FlipLeverOff();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			playerInRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			playerInRange = false;
		}
	}

	private void FlipLeverOn() {
		animator.SetBool(IsFlipped, true);
		flipped                  = true;
		platformScript.direction = 1;
	}

	private void FlipLeverOff() {
		animator.SetBool(IsFlipped, false);
		flipped                  = false;
		platformScript.direction = -1;
	}
}