using UnityEngine;

public class CrateController : MonoBehaviour {
	[SerializeField] private GameObject  destroyCrate;
	[SerializeField] private GameObject  objectInBox;
	[SerializeField] private Rigidbody2D rb;
	private                  bool        breakBoxOnLand;

	private void Update() {
		if (breakBoxOnLand) BreakBox();
		else breakBoxOnLand = rb.linearVelocityY < -15;
	}

	private void BreakBox() {
		if (rb.linearVelocityY != 0) return;
		if (objectInBox) Instantiate(objectInBox, transform.position, Quaternion.identity);

		Instantiate(destroyCrate, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("explosion")) BreakBox();
	}
}