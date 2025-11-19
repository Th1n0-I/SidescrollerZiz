using System;
using UnityEngine;

public class LockedDoor : MonoBehaviour {
	private Inventory inventory;
	private Counters  counters;
	private string    typeOfLock;
	private string    typeOfKey;

	private void Start() {
		typeOfLock = gameObject.tag;
		inventory  = FindAnyObjectByType<Inventory>();
		counters   = FindAnyObjectByType<Counters>();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			switch (typeOfLock) {
				case "BlueLock" when inventory.blueKeys > 0:
					Destroy(gameObject);
					inventory.blueKeys--;
					counters.DeleteKey("blueKey");
					break;
				case "RedLock" when inventory.redKeys > 0:
					Destroy(gameObject);
					inventory.redKeys--;
					counters.DeleteKey("redKey");
					break;
				case "YellowLock" when inventory.yellowKeys > 0:
					Destroy(gameObject);
					inventory.yellowKeys--;
					counters.DeleteKey("yellowKey");
					break;
				case "GreenLock" when inventory.greenKeys > 0:
					Destroy(gameObject);
					inventory.greenKeys--;
					counters.DeleteKey("greenKey");
					break;
			}
		}
	}
}