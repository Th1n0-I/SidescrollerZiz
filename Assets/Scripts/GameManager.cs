using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	private int   deathScreen = 8;
	public  int   currentScene;
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
		gameTimer       = 0;
		deathCounter    = 0;
		gemCounter      = 0;
		isCountingStats = true;
	}
	
	public void StopCountingStats() {
		isCountingStats = false;
	}

	public void ResumeCountingStats() {
		isCountingStats = true;
	}

	public void PlayerDied() {
		currentScene = SceneManager.GetActiveScene().buildIndex;
		deathCounter++;
		StopCountingStats();
		SceneManager.LoadScene(deathScreen);
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