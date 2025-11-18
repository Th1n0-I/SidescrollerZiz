using UnityEngine;

public class BackgroundController : MonoBehaviour {
	private float startPosX, lengthX, startPosY, lengthY;

	public GameObject camera;

	public float parallaxEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
	    startPosX = transform.position.x;
	    startPosY = transform.position.y;
	    lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
	    lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distanceX = camera.transform.position.x * parallaxEffect;
        float distanceY = camera.transform.position.y * parallaxEffect;
        float movementX = camera.transform.position.x * (1 - parallaxEffect);
        float movementY = camera.transform.position.y * (1 - parallaxEffect);
        
        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        if (movementX > startPosX + lengthX) {
	        startPosX += lengthX;
        } else if (movementX < startPosX - lengthX) {
	        startPosX -= lengthX;
        }

        if (movementY > startPosY + lengthY) {
	        startPosY += lengthY;
        } else if (movementY < startPosY - lengthY) {
	        startPosY -= lengthY;	
        }
    }
}
