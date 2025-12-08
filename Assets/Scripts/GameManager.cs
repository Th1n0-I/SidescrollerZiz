using UnityEngine;

public class GameManager : MonoBehaviour {
	public  float gameTimer;
	public  int   deathCounter;
	public  int   gemCounter;
	private bool  isCountingStats = false;
	
	
	public void Awake() {
		if (GameObject.FindGameObjectsWithTag("GameManager").Length > 1) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
		}
	}

	public void StartCountingStats() {
		isCountingStats = true;
	}
	
	public void StopCountingStats() {
		isCountingStats = false;
	}

	public void AddDeath() {
		deathCounter++;
	}
	public void AddGem() {
		gemCounter++;
	}

	public void Update() {
		if (isCountingStats) {
			gameTimer += Time.deltaTime * Time.timeScale;
			Debug.Log(gameTimer);
		}
	}
}