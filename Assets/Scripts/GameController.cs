using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VR = UnityEngine.VR;

public enum GameState {
	NONE,
	MAIN_MENU,
	PLAYING,
	PAUSED,
	GAMEOVER
};

public class GameController : MonoBehaviour {
	
	public Generator generator;
	public UIController uiDefault;
	public UIController uiOculus;

	public GameObject cameraDefault;
	public GameObject cameraOculus;
	public OVRCharacterController ovrCharacter;

	public GameObject playerSpawn;

	// Active player controller
	public GameObject player;

	// Activer UI controller
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

	private int defCurrentLevel;
	private float defMoveSpeedY;
	private float defMoveSpeedIncr;

	// Global static informations
	public static GameState gameState = GameState.NONE;
	public static GameState nextGameState = GameState.MAIN_MENU;
	public static bool isPaused = true;
	public static bool enableRift = true;

	// Use this for initialization
	void Start () {
		if (VR.VRDevice.isPresent && enableRift) {
			GameObject.Find("UIs").transform.FindChild("UI_OCULUS").gameObject.SetActive(true);
			ui = uiOculus;
			cameraOculus.SetActive(true);
			cameraDefault.SetActive(false);
			ovrCharacter.enabled = true;
		} else {
			GameObject.Find("UIs").transform.FindChild("UI").gameObject.SetActive(true);
			ui = uiDefault;
			cameraOculus.SetActive(false);
			cameraDefault.SetActive(true);
			ovrCharacter.enabled = false;
		}
		if (collisionGenerationTime == 0)
			collisionGenerationTime = 1.0f;
		defCurrentLevel = currentLevel;
		defMoveSpeedY = moveSpeedY;
		defMoveSpeedIncr = moveSpeedIncr;
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
		ui.hud.OnLevelUp (currentLevel);
		ui.hud.hudCaption.Show("Level " + currentLevel, generator.activeWorld.name);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			TogglePause();
		}
		if (!isPaused) {
			if (gameState == GameState.PLAYING) {
				moveSpeedY += moveSpeedIncr;
				timeSpent += Time.deltaTime;
				score += moveSpeedY / 1000.0f + timeSpent / 100.0f;
				if (score >= scoreTarget) {
					LevelUp ();
				}
			}
		}
		if (nextGameState != GameState.NONE && ui.uiLocked == false) {
			Debug.Log ("Gamestate updating to " + nextGameState);
			switch (nextGameState) 
			{
			case GameState.PLAYING:
				isPaused = false;
				Time.timeScale = 1.0f;
				ResetGame ();
				ui.ShowGameStatePanel (nextGameState);
				ui.FadeOut ();
				ui.hud.hudCaption.Show("Level " + currentLevel, generator.activeWorld.name);
				break;
			case GameState.MAIN_MENU:
				ResetGame();
				isPaused = false;
				Time.timeScale = 1.0f;
				ui.ShowGameStatePanel (nextGameState);
				ui.FadeOut ();
				break;
			case GameState.GAMEOVER:
				isPaused = true;
				Time.timeScale = 1.0f;
				ui.ShowGameStatePanel (nextGameState);
				ui.hud.UpdateGameOverHUD(this);
				break;
			default:
				break;
			}
			gameState = nextGameState;
			nextGameState = GameState.NONE;
		}
		ui.hud.UpdateHUD (this);
	}

	public void ResetGame() {
		currentLevel = defCurrentLevel;
		timeSpent = 0.0f;
		finalScore = 0.0f;
		score = 0.0f;
		scoreTarget = 100.0f;
		moveSpeedY = defMoveSpeedY;
		player.transform.position = playerSpawn.transform.position;
		player.GetComponent<Rigidbody> ().Sleep ();
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

	// Main callbacks
	public void OnPlayerCollision() {
		// Apply gameover
		if (gameState != GameState.PLAYING)
			return;
		nextGameState = GameState.GAMEOVER;
	}

	// Menu callbacks
	public void OnMenuPlayClicked() {
		if (ui.uiLocked)
			return;
		ui.FadeIn ();
		nextGameState = GameState.PLAYING;
		Debug.Log ("Gamestate swap started to " + nextGameState);
	}
	
	public void OnMenuExitClicked() {
		if (ui.uiLocked)
			return;
		Debug.Log ("Bye");
		//System.Environment.Exit (0);
	}
	
	public void OnMenuResumeClicked() {
		if (ui.uiLocked)
			return;
		Debug.Log ("Unpaused");
		isPaused = false;
		Time.timeScale = 1.0f;
		ui.SetPauseVisible (false);
	}
	
	public void OnMenuRetryClicked() {
		if (ui.uiLocked)
			return;
		ui.FadeIn ();
		nextGameState = GameState.PLAYING;
		Debug.Log ("Gamestate swap started to " + nextGameState);
	}
	
	public void OnMenuBackToMainMenuClicked() {
		if (ui.uiLocked)
			return;
		ui.FadeIn ();
		nextGameState = GameState.MAIN_MENU;
		Debug.Log ("Gamestate swap started to " + nextGameState);
	}
}
