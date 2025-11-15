using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lever : MonoBehaviour {
	private static readonly  int            IsFlipped = Animator.StringToHash("isFlipped");
	public                   bool           flipped   = false;
	private                  Animator       animator;
	public                   bool           playerInRange;
	[SerializeField] private InputAction    interact;
	[SerializeField] private GameObject     keycap;
	private                  SpriteRenderer keycapSr;
	[SerializeField] private GameObject     player;

	private void Start() {
		animator = gameObject.GetComponent<Animator>();
		interact = InputSystem.actions.FindAction("Interact");
		keycapSr = keycap.GetComponent<SpriteRenderer>();
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
			if (!flipped) {
				FlipLeverOn();
			}
			else {
				FlipLeverOff();
			}
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

	public void FlipLeverOn() {
		animator.SetBool(IsFlipped, true);
		flipped = true;
	}
	public void FlipLeverOff() {
		animator.SetBool(IsFlipped, false);
		flipped = false;
	}
}
