using UnityEngine;

public class FlyingEnemyHitDetection : MonoBehaviour {
	private FlyingEnemy    enemy;
	private PlayerMovement pMovement;

	private void Awake() {
		enemy     = GetComponentInParent<FlyingEnemy>();
		pMovement = FindAnyObjectByType<PlayerMovement>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player") && pMovement.invulnerabilityTimer <= 0) {
			enemy.HitEnemy();
			pMovement.Bounce();
		}
	}
}