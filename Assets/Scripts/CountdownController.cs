using UnityEngine;

public class CountdownController : MonoBehaviour {
	private Transform bombTransform;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start() {
		bombTransform = GetComponentInParent<Transform>();
	}

	// Update is called once per frame
	void Update() {
		gameObject.transform.position = bombTransform.position - new Vector3(0, -1.2f, 0);
	}
}