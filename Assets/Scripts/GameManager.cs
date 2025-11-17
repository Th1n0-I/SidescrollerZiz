using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private                  InputAction pauseAction;
	[SerializeField] private GameObject  pauseMenu;
	private                  bool        gameIsPaused = false;
	private                  Button      continueButton;
	private                  Button      menuButton;
	private                  Button      startButton;
	private                  Button      playgroundButton;
	private                  Button      quitButton;

	private void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
		DontDestroyOnLoad(gameObject);
		;
	}

	private void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	
	private void Update() {
		if (SceneManager.GetActiveScene().name == "PlayGround") {
			if (pauseAction.WasPerformedThisFrame() && !gameIsPaused) {
				PauseGame();
			} else if (pauseAction.WasPerformedThisFrame()) {
				UnPauseGame();
			}
		} else if (SceneManager.GetActiveScene().name == "StartMenu") {
			
		}
	}

	

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log(scene.name);
		if (scene.name == "StartScene") {
			SceneManager.LoadScene("StartMenu");
		}
		else if (scene.name == "PlayGround") {
			pauseAction = InputSystem.actions["Pause"];
			pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
			
			continueButton = GameObject.FindGameObjectWithTag("ContinueButton").GetComponent<Button>();
			menuButton = GameObject.FindGameObjectWithTag("MenuButton").GetComponent<Button>();
			
			continueButton.onClick.AddListener(UnPauseGame);
			menuButton.onClick.AddListener(ReturnToMenu);
			
			pauseMenu.SetActive(false);
		} else if (scene.name == "StartMenu") {
			startButton = GameObject.FindGameObjectWithTag("StartButton").GetComponent<Button>();
			quitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
			playgroundButton = GameObject.FindGameObjectWithTag("PlaygroundButton").GetComponent<Button>();

			Debug.Log(startButton);
			Debug.Log(quitButton);
			Debug.Log(playgroundButton);
			
			startButton.onClick.AddListener(StartGame);
			playgroundButton.onClick.AddListener(Playground);
			quitButton.onClick.AddListener(Quit);
		}
	}
	
	

	private void PauseGame() {
		Time.timeScale = 0;
		pauseMenu.SetActive(true);
		gameIsPaused = true;
	}

	public void UnPauseGame() {
		Time.timeScale = 1;
		pauseMenu.SetActive(false);
		gameIsPaused = false;
	}

	private void ReturnToMenu() {
		continueButton.onClick.RemoveAllListeners();
		menuButton.onClick.RemoveAllListeners();
		SceneManager.LoadScene("StartMenu");
	}

	public void Quit() {
		Application.Quit();
	}

	public void StartGame() {
		
	}

	public void Playground() {
		startButton.onClick.RemoveAllListeners();
		playgroundButton.onClick.RemoveAllListeners();
		quitButton.onClick.RemoveAllListeners();
		SceneManager.LoadScene("Playground");
	}
}
