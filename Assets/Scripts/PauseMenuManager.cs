using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {
	private                  bool        paused = false;
	[SerializeField] private int         startMenu;
	[SerializeField] private InputAction pauseAction;
	[SerializeField] private GameObject  pauseMenuContainer;
	private                  GameObject  player;


	private void OnEnable() {
		pauseAction = InputSystem.actions.FindAction("Pause");
		player      = GameObject.FindWithTag("Player");
	}

	private void Update() {
		if (paused && pauseAction.WasPressedThisFrame()) {
			Debug.Log("EscapePressed");
			ResumeGame();
		}
		else if (!paused && pauseAction.WasPressedThisFrame()) {
			Debug.Log("EscapePressed");
			PauseGame();
		}
	}

	private void PauseGame() {
		Time.timeScale = 0;
		paused         = true;
		pauseMenuContainer.SetActive(true);
	}

	public void ResumeGame() {
		Debug.Log("Resuming");
		Time.timeScale = 1;
		paused         = false;
		pauseMenuContainer.SetActive(false);
	}

	public void ReturnToMainMenu() {
		Debug.Log("Returning to main menu");
		Destroy(player);
		Time.timeScale = 1;
		SceneManager.LoadScene(startMenu);
	}
}