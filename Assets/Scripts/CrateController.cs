using UnityEngine;

public class CrateController : MonoBehaviour {
	[SerializeField] private GameObject groundCheckPosition;
	[SerializeField] private float      groundCheckRadius;
	[SerializeField] private LayerMask  groundLayer;
	[SerializeField] private float      fallStartPos;
	[SerializeField] private float      fallLength;
	[SerializeField] private bool       countedFallStartPos = false;
	[SerializeField] private float      maxFallLength;
	[SerializeField] private bool       isGrounded;
	[SerializeField] private GameObject destroyCrate;

	void Update() {
		GroundCheck();
	}

	private void GroundCheck() {
		Collider2D hit =
			Physics2D.OverlapCircle(groundCheckPosition.transform.position, groundCheckRadius, groundLayer);

		if (hit) {
			isGrounded = true;
		}
		else {
			isGrounded = false;
		}

		if (!hit && !countedFallStartPos) {
			fallStartPos        = transform.position.y;
			countedFallStartPos = true;
		}
		else if (hit && countedFallStartPos) {
			fallLength          = fallStartPos - transform.position.y;
			countedFallStartPos = false;
			if (fallLength >= maxFallLength) {
				BreakBox();
			}
		}
	}

	private void BreakBox() {
		GameObject.Instantiate(destroyCrate, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	private void OnDrawGizmos() {
		if (isGrounded) {
			Gizmos.color = Color.green;
		}
		else {
			Gizmos.color = Color.red;
		}

		Gizmos.DrawWireSphere(groundCheckPosition.transform.position, groundCheckRadius);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("explosion")) {
			BreakBox();
		}
	}
}