using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	[Header("Scenes")] [SerializeField] int playgroundScene;

	public void QuitGame() {
		Application.Quit();
	}

	public void LoadPlaygroundScene() {
		SceneManager.LoadScene(playgroundScene);
	}

	public void LoadLevel1() {
		
	}
}