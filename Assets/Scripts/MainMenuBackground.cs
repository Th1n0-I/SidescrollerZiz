using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackground : MonoBehaviour {
	private                  float startPos;
	private                  float distance;
	private                  float movement;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float parallaxEffect;
	private                  float imageWidth = 3600f * 0.7f;
    private void Start() {
	    startPos = transform.position.x;
    }

    private void Update() {
	    distance = transform.position.x + moveSpeed * Time.deltaTime * parallaxEffect;
	    movement = distance - startPos;
		
	    transform.position = new Vector3(distance, transform.position.y, transform.position.z);
	    
	    if (movement > imageWidth) {
			distance -= imageWidth;   
			transform.position = new Vector3(distance, transform.position.y, transform.position.z);
	    } else if (movement < -imageWidth) {
		    distance += imageWidth;
		    transform.position = new Vector3(distance, transform.position.y, transform.position.z);
	    }
    }
}
