using System.Collections;
using UnityEngine;

public class CratePartController : MonoBehaviour {
	[SerializeField] private GameObject     part1;
	[SerializeField] private GameObject     part2;
	[SerializeField] private GameObject     part3;
	[SerializeField] private GameObject     part4;
	private                  Rigidbody2D    part1Rb;
	private                  Rigidbody2D    part2Rb;
	private                  Rigidbody2D    part3Rb;
	private                  Rigidbody2D    part4Rb;
	private                  SpriteRenderer part1Sr;
	private                  SpriteRenderer part2Sr;
	private                  SpriteRenderer part3Sr;
	private                  SpriteRenderer part4Sr;

	[SerializeField] private float explosionRadius;
	[SerializeField] private float explosionForce;

	void Start() {
		part1Rb = part1.GetComponent<Rigidbody2D>();
		part2Rb = part2.GetComponent<Rigidbody2D>();
		part3Rb = part3.GetComponent<Rigidbody2D>();
		part4Rb = part4.GetComponent<Rigidbody2D>();

		part1Rb.AddForceAtPosition(new Vector2(3,  -3), transform.position, ForceMode2D.Impulse);
		part2Rb.AddForceAtPosition(new Vector2(3,  3), transform.position, ForceMode2D.Impulse);
		part3Rb.AddForceAtPosition(new Vector2(-3, 3), transform.position, ForceMode2D.Impulse);
		part4Rb.AddForceAtPosition(new Vector2(-3, -3), transform.position, ForceMode2D.Impulse);

		part1Sr = part1.GetComponent<SpriteRenderer>();
		part2Sr = part2.GetComponent<SpriteRenderer>();
		part3Sr = part3.GetComponent<SpriteRenderer>();
		part4Sr = part4.GetComponent<SpriteRenderer>();

		StartCoroutine(DestroyParts());
	}

	IEnumerator DestroyParts() {
		yield return new WaitForSeconds(3f);
		Destroy(part1);
		Destroy(part2);
		Destroy(part3);
		Destroy(part4);
	}

	// Update is called once per frame
	void Update() {
		if (part1Sr) {
			var color = part1Sr.color;
			color.a       = color.a - 1f * Time.deltaTime / 2;
			part1Sr.color = color;
			part2Sr.color = color;
			part3Sr.color = color;
			part4Sr.color = color;
		}
	}
}