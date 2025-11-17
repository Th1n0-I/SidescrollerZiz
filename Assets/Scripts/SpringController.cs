using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpringController : MonoBehaviour
{
	private static readonly  int            IsActivated = Animator.StringToHash("IsActivated");
	private                  bool           isActivated = false;
	private                  Animator       animator;
	[SerializeField] private PlayerMovement playerMovement;

	private void Start() {
		animator = GetComponent<Animator>();
		playerMovement = FindAnyObjectByType<PlayerMovement>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player") && !isActivated) {
			playerMovement.Bounce(2);
			animator.SetBool(IsActivated, true);
			isActivated = true;
			StartCoroutine(ActivatedTimer());
		}
	}
	IEnumerator ActivatedTimer() {
		yield return new WaitForSeconds(0.5f);
		animator.SetBool(IsActivated, false);
		isActivated = false;
	}
}
