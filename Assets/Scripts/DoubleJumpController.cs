using UnityEngine;

public class DoubleJumpController : MonoBehaviour {
	[SerializeField] private GameObject     collectible;
	
	public void Collected() {
		collectible.gameObject.SetActive(false);
	}

	public void PlayerLanded() {
		collectible.gameObject.SetActive(true);
	}
}
