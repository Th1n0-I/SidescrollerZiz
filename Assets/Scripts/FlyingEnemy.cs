using System.Collections;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour {
	private static readonly int IsDead = Animator.StringToHash("isDead");
	BoxCollider2D               enemyBc;
	Rigidbody2D                 enemyRb;
	Animator                    animator;
	SpriteRenderer              enemySr;
	[SerializeField] GameObject pointA;
	[SerializeField] GameObject pointB;

	[SerializeField] float cycleTime;

	public float moveDistance;
	public float distanceBetweenPoints;


	// 1: Moves to the right
	// -1: Moves ot the left
	public int direction = 1;

	Vector3 pointAPos;

	Vector3 pointBPos;

	[SerializeField] float jumpForce;

	private void Update() {
		MoveEnemy();
	}

	public void Awake() {
		pointAPos = pointA.transform.position;
		pointBPos = pointB.transform.position;

		enemyBc  = GetComponent<BoxCollider2D>();
		enemyRb  = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		enemySr  = GetComponent<SpriteRenderer>();

		distanceBetweenPoints = pointBPos.x - pointAPos.x;

		CalculateTrajectory();
	}

	public void HitEnemy() {
		enemyBc.enabled = false;
		animator.SetBool(IsDead, true);
		enemyRb.gravityScale = 4;
		enemyRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		StartCoroutine(KillEnemy());
	}

	IEnumerator KillEnemy() {
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}

	private void CalculateTrajectory() {
		moveDistance = distanceBetweenPoints / cycleTime;
	}

	private void MoveEnemy() {
		gameObject.transform.position += new Vector3(moveDistance * Time.deltaTime * direction, 0, 0);


		if (gameObject.transform.position.x <= pointAPos.x) {
			gameObject.transform.position = new Vector3(pointAPos.x, gameObject.transform.position.y, 0);
			direction                     = 1;
			enemySr.flipX                 = true;
		}
		else if (gameObject.transform.position.x >= pointBPos.x) {
			gameObject.transform.position = new Vector3(pointBPos.x, gameObject.transform.position.y, 0);
			direction                     = -1;
			enemySr.flipX                 = false;
		}
	}
}