using UnityEngine;

public class CrateController : MonoBehaviour {
	[SerializeField] private GameObject  destroyCrate;
	[SerializeField] private GameObject  objectInBox;
	private                  bool        breakBoxOnLand;
	private                  Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}
	
	private void Update() {
		BreakCheck();
	}

	private void BreakCheck() {
		if (!breakBoxOnLand) {
			breakBoxOnLand = rb.linearVelocityY < -10;
		} else if (breakBoxOnLand) {
			if (rb.linearVelocityY == 0) BreakBox();
		}
	}

	private void BreakBox() {
		if (objectInBox) {
			GameObject.Instantiate(objectInBox, transform.position, Quaternion.identity);
		}
		GameObject.Instantiate(destroyCrate, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("explosion")) {
			BreakBox();
		}
	}
}