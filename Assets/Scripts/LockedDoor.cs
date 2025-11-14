using System;
using UnityEngine;

public class LockedDoor : MonoBehaviour {
	private Inventory inventory;
	private string    typeOfLock;
	private string    typeOfKey;

	private void Start() {
		typeOfLock = gameObject.tag;
		inventory  = FindAnyObjectByType<Inventory>();
		
		
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
	        switch (typeOfLock) {
		        case "BlueLock" when inventory.blueKeys > 0:
			        Destroy(gameObject);
			        inventory.blueKeys--;
			        break;
		        case "RedLock" when inventory.redKeys > 0:
			        Destroy(gameObject);  
			        inventory.redKeys--;
			        break;
		        case "YellowLock" when inventory.yellowKeys > 0:
			        Destroy(gameObject); 
			        inventory.yellowKeys--;
			        break;
		        case "GreenLock" when inventory.greenKeys > 0:
			        Destroy(gameObject);   
			        inventory.greenKeys--;
			        break;
	        }
        }
    }
}
