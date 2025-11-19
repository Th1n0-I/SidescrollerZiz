using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {
	private                  bool        paused = false;
	[SerializeField] private int         startMenu;
	[SerializeField] private InputAction pauseAction;
	[SerializeField] private GameObject  pauseMenuContainer;
	[SerializeField] private GameObject  player;


	private void OnEnable() {
		pauseAction = InputSystem.actions.FindAction("Pause");
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

	public void PauseGame() {
		Time.timeScale = 0;
		paused         = true;
		pauseMenuContainer.SetActive(true);
	}

	public void ResumeGame() {
		Time.timeScale = 1;
		paused         = false;
		pauseMenuContainer.SetActive(false);
	}

	public void ReturnToMainMenu() {
		Destroy(player);
		Time.timeScale = 1;
		SceneManager.LoadScene(startMenu);
	}
}