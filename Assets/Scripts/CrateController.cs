using UnityEngine;

public class CrateController : MonoBehaviour {
	[SerializeField] private GameObject groundCheckPosition;
	[SerializeField] private float      groundCheckRadius;
	[SerializeField] private LayerMask  groundLayer;
	private                  float      fallStartPos;
	private                  float      fallLength;
	private                  bool       countedFallStartPos = false;
	[SerializeField] private float      maxFallLength;
	
    void Update()
    {
        GroundCheck();
    }

    private void GroundCheck() {
	    Collider2D hit = Physics2D.OverlapCircle(groundCheckPosition.transform.position, groundCheckRadius, groundLayer);
	    if (!hit && !countedFallStartPos) {
		    fallStartPos = transform.position.y;
		    countedFallStartPos = true;
	    } else if (!hit && countedFallStartPos) {
		    fallLength = fallStartPos -  transform.position.y;
		    countedFallStartPos = false;
		    if (fallLength >= maxFallLength) {
			    BreakBox();
		    }
	    }
    }

    private void BreakBox() {
	    Destroy(gameObject);
    }
}
