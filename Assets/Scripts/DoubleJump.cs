using UnityEngine;

public class DoubleJump : MonoBehaviour {
	[SerializeField] private GameObject doubleJumpCollectible;

	void Start() {
		SpawnCollectible();
	}

	public void SpawnCollectible() {
		GameObject x = Instantiate(doubleJumpCollectible, transform, true);
		x.transform.position = transform.position;
	}
}