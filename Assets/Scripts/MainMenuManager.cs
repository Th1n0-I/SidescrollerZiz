using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	[Header("Scenes")] [SerializeField] int        level1;
	public                              GameObject gameManager;

	public void Start() {
		gameManager = GameObject.Find("GameManager");
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void LoadLevel1() {
		gameManager.GetComponent<GameManager>().StartCountingStats();
		SceneManager.LoadScene(level1);
	}
}