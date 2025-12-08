using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenController : MonoBehaviour
{
	[SerializeField] private int      mainMenu;
	[SerializeField] private TMP_Text deathCounterText;
	[SerializeField] private TMP_Text gemCounterText;
	[SerializeField] private TMP_Text timerText;

	private GameObject  gameManager;
	private GameManager gameManagerScript;
	private string      timer;

	private int seconds;
	private int minutes;

	public void Start() {
		if (GameObject.Find("GameManager") != null) {
			gameManager       = GameObject.Find("GameManager");
			gameManagerScript = gameManager.GetComponent<GameManager>();
			
			seconds           = Convert.ToInt32(gameManagerScript.gameTimer % 60);
			minutes           = Convert.ToInt32(gameManagerScript.gameTimer - seconds) / 60;
		
			deathCounterText.text = "X"                + gameManagerScript.deathCounter;
			gemCounterText.text   = "X"                + gameManagerScript.gemCounter;
			timerText.text        = minutes.ToString() + ":" + seconds.ToString();
		}
	}
	
	public void Menu() {
		SceneManager.LoadScene(mainMenu);
	}

	public void QuitGame() {
		Application.Quit();
	}
}
