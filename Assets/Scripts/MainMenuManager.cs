using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	[Header("Scenes")] [SerializeField] int level1;

	public void QuitGame() {
		Application.Quit();
	}

	public void LoadLevel1() {
		SceneManager.LoadScene(level1);
	}
}