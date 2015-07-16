using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameState {
	MAIN_MENU,
	PLAYING,
	PAUSED,
	GAMEOVER
};

public class GameController : MonoBehaviour {

	public Generator generator;
	public HUDController hud;
	public UIController ui;

	// Parameters of the game
	public float collisionGenerationTime = 1.0f;
	public float moveSpeedY = 1.0f;
	public float offsetY;
	public float moveSpeedIncr = 0.0035f;

	public int currentLevel = 1;
	public float timeSpent = 0.0f;
	public float finalScore = 0.0f;
	public float score = 0.0f;
	public float scoreTarget = 100.0f;

	// Global static informations
	public static GameState gameState = GameState.MAIN_MENU;
	public static bool isPaused = true;

	// Use this for initialization
	void Start () {
		if (collisionGenerationTime == 0)
			collisionGenerationTime = 1.0f;
		timeSpent = 0.0f;
		ui.ShowGameStatePanel (gameState);
	}

	void PauseGame() {

	}

	void StopGame() {

	}

	void StartGame() {

	}

	void LevelUp() {
		currentLevel++;
		finalScore += score;
		score = 0;
		scoreTarget *= 2;
		hud.OnLevelUp (currentLevel);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			TogglePause();
		}
		if (!isPaused) {
			moveSpeedY += moveSpeedIncr;
			timeSpent += Time.deltaTime;
			score += moveSpeedY / 1000.0f + timeSpent / 100.0f;
			if (score >= scoreTarget) {
				LevelUp ();
			}
		}
		hud.UpdateHUD (this);
	}

	public void ResetGame() {
		generator.Clear ();
		generator.InitialSpawn ();
	}

	public void TogglePause() {
		if (gameState != GameState.PLAYING)
			return;
		isPaused = !isPaused;
		ui.SetPauseVisible (isPaused);
		if (isPaused) {
			Time.timeScale = 0.0f;	
		} else {
			Time.timeScale = 1.0f;	
		}
	}

	// Menu callbacks
	public void OnMenuPlayClicked() {
		isPaused = false;
		Time.timeScale = 1.0f;
		gameState = GameState.PLAYING;
		ResetGame ();
		ui.ShowGameStatePanel (gameState);
	}
	
	public void OnMenuExitClicked() {
		System.Environment.Exit (0);
	}
	
	public void OnMenuResumeClicked() {
		isPaused = false;
		Time.timeScale = 1.0f;
		ui.SetPauseVisible (false);
	}
	
	public void OnMenuRetryClicked() {
		gameState = GameState.PLAYING;
		ui.ShowGameStatePanel (gameState);
	}
	
	public void OnMenuBackToMainMenuClicked() {
		gameState = GameState.MAIN_MENU;
		ui.ShowGameStatePanel (gameState);
	}
}
