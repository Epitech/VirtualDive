using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VR = UnityEngine.VR;

public enum GameState {
	NONE,
	MAIN_MENU,
    OPTIONS,
    SCORE,
	PLAYING,
	PAUSED,
	GAMEOVER
};

public enum InputType {
    GYRO = 0,
    TOUCH = 1,
    DPAD = 2,
    MAX
};

public enum Sensibility {
    LOW = 0,
    MEDIUM = 1,
    HIGH = 2,
    MAX
}

public class GameController : MonoBehaviour {
	
	public Generator generator;
    public GameDataController gameDataSerializer;

	public GameObject cameraDefault;
	public GameObject cameraOculus;
	public OVRCharacterController ovrCharacter;

	public GameObject playerSpawn;

	// Active player controller
	public GameObject player;

	// Activer UI controller
	public UIController ui;
    public UIControllerOculus uiOculus;

	// Parameters of the game
	public float collisionGenerationTime = 1.0f;
	public float moveSpeedY = 1.0f;
	public float offsetY;
	public float moveSpeedIncr = 0.0035f;
    public float invulnerability = 0.0f;
    public float hitDelay = 0.0f;
    public float defHitDelay = 2.0f;

	public int currentLevel = 1;
	public float timeSpent = 0.0f;
	public float finalScore = 0.0f;
	public float score = 0.0f;
	public float scoreTarget = 100.0f;
    public float damage = 0.0f;
    public float maxDamage = 100.0f;

	private int defCurrentLevel;
	private float defMoveSpeedY;
	private float defMoveSpeedIncr;

    // Cinematic animations parameters
    public CinematicController cinematicsController;
    public AnimationClip startAnimation;

	// Global static informations
	public static GameState gameState = GameState.NONE;
	public static GameState nextGameState = GameState.MAIN_MENU;
	public static bool isPaused = true;

    public static bool soundState = true;
    public static bool musicState = true;
    public static Sensibility sensibility = Sensibility.LOW;
    public static InputType activeInput = InputType.TOUCH;

	public bool enableRift = true;

    // After start cinematic is done
    void OnStartCinematicFinished()
    {
        player.GetComponent<MovementController>().isMovementAllowed = true;
    }

	// Use this for initialization
	void Start () {

        gameDataSerializer = new GameDataController();
        gameDataSerializer.LoadGamedata(this);

        // Conditionnal - Android setup
        if (Application.platform == RuntimePlatform.Android)
        {
            cameraDefault.GetComponent<UnityStandardAssets.ImageEffects.Tonemapping>().enabled = false;
        }

        if (VR.VRDevice.isPresent && enableRift) {
			GameObject.Find("UIs").transform.FindChild("UI_OCULUS").gameObject.SetActive(true);
			cameraOculus.SetActive(true);
			cameraDefault.SetActive(false);
			ovrCharacter.enabled = true;
            ui.panelFade = ui.oculusPanelFade;
            //GameObject.Find("OVRPlayerController").transform.GetComponentInChildren<Rigidbody>().drag = 10.0f;
		} else {
            GameObject.Find("UIs").transform.FindChild("UI_OCULUS").gameObject.SetActive(false);
			cameraOculus.SetActive(false);
			cameraDefault.SetActive(true);
			ovrCharacter.enabled = false;
            //GameObject.Find("OVRPlayerController").transform.GetComponentInChildren<Rigidbody>().drag = 1.0f;
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
        ui.hud.OnLevelUp(currentLevel);
        ui.hud.hudCaption.Show("Level " + currentLevel, generator.activeWorld.name);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			TogglePause();
		}
		if (!isPaused) {
			if (gameState == GameState.PLAYING) {
				moveSpeedY += ApplyTimeScale(moveSpeedIncr);
				timeSpent += Time.deltaTime;
				score += ApplyTimeScale(moveSpeedY / 1000.0f + timeSpent / 100.0f);
				if (score >= scoreTarget) {
					LevelUp ();
				}

                // Invulnerability reduction
                invulnerability -= ApplyTimeScale(1.0f / 60.0f);
                if (invulnerability < 0)
                    invulnerability = 0.0f;

                // Damage time reduction
                if (hitDelay == 0.0f)
                {
                    damage -= ApplyTimeScale(10.0f / 60.0f);
                }
                else
                {
                    hitDelay -= ApplyTimeScale(1.0f / 60.0f);
                    if (hitDelay < 0.0f)
                        hitDelay = 0.0f;
                }

                if (damage < 0)
                    damage = 0.0f;
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
                ui.HideAll();
                ui.ShowGameStatePanel(nextGameState);
				ui.FadeOut ();
                player.GetComponent<MovementController>().isMovementAllowed = false;
                cinematicsController.RunAnimation(player, startAnimation, OnStartCinematicFinished);
				//ui.hud.hudCaption.Show("Level " + currentLevel, generator.activeWorld.name);
				break;
			case GameState.MAIN_MENU:
				ResetGame();
				isPaused = false;
				Time.timeScale = 1.0f;
                ui.HideAll();
				ui.ShowGameStatePanel (nextGameState);
				ui.FadeOut ();
				break;
			case GameState.GAMEOVER:
				isPaused = true;
				Time.timeScale = 1.0f;
                ui.HideAll();
                ui.ShowGameStatePanel(nextGameState);
				ui.hud.UpdateGameOverHUD(this);
                gameDataSerializer.SaveGamedata(this);
				break;
			default:
				break;
			}
			gameState = nextGameState;
			nextGameState = GameState.NONE;
		}
        ui.hud.UpdateHUD(this);
	}

	public void ResetGame() {
		currentLevel = defCurrentLevel;
		timeSpent = 0.0f;
		finalScore = 0.0f;
		score = 0.0f;
		scoreTarget = 100.0f;
        damage = 0.0f;
        invulnerability = 0.0f;
		moveSpeedY = defMoveSpeedY;
		player.transform.position = playerSpawn.transform.position;
		//player.GetComponent<Rigidbody> ().Sleep ();
		generator.Clear ();
		generator.InitialSpawn ();
        player.GetComponent<MovementController>().Reposition();
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
        if (gameState != GameState.PLAYING || invulnerability > 0.0f)
			return;
        damage += Random.Range(20.0f, 30.0f);
        hitDelay = defHitDelay;
        invulnerability = 0.3f;
        if (damage > 100.0f)
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

    public void OnOptionsBackToMainMenuClicked()
    {
        if (ui.uiLocked)
            return;
        gameState = GameState.MAIN_MENU;
        ui.HideAll();
        ui.ShowGameStatePanel(gameState);
        Debug.Log("Gamestate swap to " + gameState);
    }

    public void OnOptionsClicked()
    {
        if (ui.uiLocked)
            return;
        gameState = GameState.OPTIONS;
        ui.HideAll();
        ui.ShowGameStatePanel(gameState);
        Debug.Log("Gamestate swap to " + gameState);
    }

    public void OnScoresClicked()
    {
        if (ui.uiLocked)
            return;
        gameState = GameState.SCORE;
        ui.HideAll();
        ui.ShowGameStatePanel(gameState);
        Debug.Log("Gamestate swap to " + gameState);
    }

    public void OnLeaderboardClicked()
    {
        if (ui.uiLocked)
            return;
        gameState = GameState.SCORE;
        ui.HideAll();
        ui.ShowGameStatePanel(gameState);
        Debug.Log("Gamestate swap to " + gameState);
    }

	public float ApplyTimeScale(float value) {
		return (Time.deltaTime / (1.0f / 60.0f) * value);
	}
}
