using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour {
	public                   Counters    counters;
	[SerializeField] private GameObject  particles;
	private                  GameManager gameManager;

	private void Start() {
		counters = FindAnyObjectByType<Counters>();
		if (GameObject.FindGameObjectWithTag("GameManager") != null) {
			gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			counters.AddDiamonds();
			GameObject.Instantiate(particles, transform.position, Quaternion.identity);
			FindAnyObjectByType<AudioManager>().PlaySound(3);
			gameManager.AddGem();
			Destroy(gameObject);
			
		}
	}
}