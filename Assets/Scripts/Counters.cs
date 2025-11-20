using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Counters : MonoBehaviour {
	private Inventory inventory;

	[SerializeField] private int diamonds;
	[SerializeField] private int count;

	[SerializeField] private GameObject       diamondCounter;
	[SerializeField] private GameObject       bombCounter;
	[SerializeField] public  List<GameObject> keys;
	[SerializeField] private GameObject       keyCounterLocation;
	[SerializeField] private GameObject       guiKeyRed;
	[SerializeField] private GameObject       guiKeyBlue;
	[SerializeField] private GameObject       guiKeyGreen;
	[SerializeField] private GameObject       guiKeyYellow;

	private GameObject x;

	public void AddDiamonds() {
		diamonds++;
	}

	void Start() {
		inventory = FindAnyObjectByType<Inventory>();
	}

	public void AddKey(string key) {
		switch (key) {
			case "redKey":
				keys.Add(GameObject.Instantiate(guiKeyRed, keyCounterLocation.transform, true));
				break;
			case "blueKey":
				keys.Add(GameObject.Instantiate(guiKeyBlue, keyCounterLocation.transform, true));
				break;
			case "greenKey":
				keys.Add(GameObject.Instantiate(guiKeyGreen, keyCounterLocation.transform, true));
				break;
			case "yellowKey":
				keys.Add(GameObject.Instantiate(guiKeyYellow, keyCounterLocation.transform, true));
				break;
		}

		for (int i = 0; i < keys.Count; i++) {
			keys[i].transform.position = new Vector3(keyCounterLocation.transform.position.x - 60 * i,
			                                         keyCounterLocation.transform.position.y, 0);
		}
	}

	public void DeleteKey(string key) {
		switch (key) {
			case "redKey":
				x = (keys.Find(x => x.gameObject.name == "GuiKeyRed(Clone)"));
				keys.Remove(x);
				Destroy(x);
				break;
			case "blueKey":
				x = (keys.Find(x => x.gameObject.name == "GuiKeyBlue(Clone)"));
				keys.Remove(x);
				Destroy(x);
				break;
			case "greenKey":
				x = (keys.Find(x => x.gameObject.name == "GuiKeyGreen(Clone)"));
				keys.Remove(x);
				Destroy(x);
				break;
			case "yellowKey":
				x = (keys.Find(x => x.gameObject.name == "GuiKeyYellow(Clone)"));
				keys.Remove(x);
				Destroy(x);
				break;
		}

		for (int i = 0; i < keys.Count; i++) {
			keys[i].transform.position = new Vector3(keyCounterLocation.transform.position.x - 60 * i,
			                                         keyCounterLocation.transform.position.y, 0);
		}
	}

	void Update() {
		if (diamonds < 1) {
			diamondCounter.SetActive(false);
		}
		else if (diamonds > 0) {
			diamondCounter.SetActive(true);
			diamondCounter.GetComponentInChildren<TextMeshProUGUI>().text = "X" + diamonds;
		}

		if (inventory.bombs < 1) {
			bombCounter.SetActive(false);
		}
		else if (inventory.bombs > 0) {
			bombCounter.SetActive(true);
			bombCounter.GetComponentInChildren<TextMeshProUGUI>().text    = "x" + inventory.bombs;
			
		}
	
		
	}
}