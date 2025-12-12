using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour {
	private GameObject gameManager;

	private void Start() {
		gameManager = GameObject.Find("GameManager");
	}
	
	public void RestartLevel() {
		gameManager.GetComponent<GameManager>().ResumeCountingStats();
		SceneManager.LoadScene(gameManager.GetComponent<GameManager>().currentScene);
	}

	public void MainMenu() {
		SceneManager.LoadScene(0);
	}
}
