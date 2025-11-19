using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour {
	public int blueKeys;
	public int yellowKeys;
	public int greenKeys;
	public int redKeys;
	public int bombs;

	private Counters counters;

	InputAction                 attackAction;
	[SerializeField] GameObject bomb;

	private void Start() {
		attackAction = InputSystem.actions.FindAction("Attack");
		counters     = FindAnyObjectByType<Counters>();
	}

	private void Update() {
		if (attackAction.WasPerformedThisFrame() && bombs > 0) {
			Instantiate(bomb, transform.position, transform.rotation);
			bombs--;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		switch (collision.gameObject.tag) {
			case "BlueKey":
				blueKeys++;
				Destroy(collision.gameObject);
				counters.AddKey("blueKey");
				break;
			case "YellowKey":
				yellowKeys++;
				Destroy(collision.gameObject);
				counters.AddKey("yellowKey");
				break;
			case "GreenKey":
				greenKeys++;
				Destroy(collision.gameObject);
				counters.AddKey("greenKey");
				break;
			case "RedKey":
				redKeys++;
				Destroy(collision.gameObject);
				counters.AddKey("redKey");
				break;
			case "Bomb":
				bombs++;
				Destroy(collision.gameObject);
				break;
		}
	}
}