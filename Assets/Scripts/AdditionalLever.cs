using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AdditionalLever : MonoBehaviour {
	private static readonly int            IsFlipped = Animator.StringToHash("isFlipped");
	private                 Lever          lever;
	private                 bool           playerInRange;
	private                 InputAction    interact;
	private                 SpriteRenderer keycapSr;
	private                 GameObject     player;
	private                 Animator       animator;

	[SerializeField]
	private GameObject     keycap;

	private void Start() {
		lever    = GetComponentInParent<Lever>();
		interact = InputSystem.actions.FindAction("Interact");
		player   = GameObject.FindGameObjectWithTag("Player");
		keycapSr = keycap.GetComponent<SpriteRenderer>();
		animator = gameObject.GetComponent<Animator>();
	}

	
	
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			playerInRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			playerInRange = false;
		}
	}

	private void CheckPlayerInputs() {
		if (interact.WasPerformedThisFrame()) {
			lever.FlipLever();
			switch (animator.GetBool(IsFlipped)) {
				case(true):
					animator.SetBool(IsFlipped, false);
					break;
				case(false):
					animator.SetBool(IsFlipped, true);
					break;
			}
		}
	}
	
	private void KeycapOpacity() {
		var color = keycapSr.color;
		color.a        = 1.5f - Vector2.Distance(player.transform.position, keycap.transform.position) / 3;
		keycapSr.color = color;
	}

	private void Update() {
		KeycapOpacity();
		if (playerInRange) {
			CheckPlayerInputs();
		}
		
	}
}
