using UnityEngine;

public class Platform : MonoBehaviour {
	[SerializeField] private int        cycleTime;
	[SerializeField] private GameObject pointA;
	[SerializeField] private GameObject pointB;

	private float moveSpeed;

	//1 = up
	//-1 = down
	public  int   direction;
	private float pointAPosY;
	private float pointBPosY;

	void Start() {
		pointAPosY = pointA.transform.position.y;
		pointBPosY = pointB.transform.position.y;
		moveSpeed  = Vector2.Distance(pointA.transform.position, pointB.transform.position) / cycleTime;
	}

	// Update is called once per frame
	void Update() {
		MovePlatform();
	}

	private void MovePlatform() {
		if (direction == 1 && gameObject.transform.position.y < pointBPosY) {
			gameObject.transform.position += new Vector3(0, moveSpeed * Time.deltaTime * direction, 0);
		}
		else if (direction == -1 && gameObject.transform.position.y > pointAPosY) {
			gameObject.transform.position += new Vector3(0, moveSpeed * Time.deltaTime * direction, 0);
		}
	}
}