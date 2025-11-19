using UnityEngine;

public class EnemyHitDetection : MonoBehaviour {
	[SerializeField] Enemy          enemy;
	[SerializeField] PlayerMovement playerMovement;

	private void Start() {
		enemy          = GetComponentInParent<Enemy>();
		playerMovement = FindAnyObjectByType<PlayerMovement>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player") && playerMovement.invulnerabilityTimer <= 0) {
			enemy.HitEnemy();
			playerMovement.Bounce();
		}
	}
}