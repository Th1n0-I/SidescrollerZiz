using Unity.VisualScripting;
using UnityEngine;

public class SpikeBlockController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 100;
		}
	}
}
