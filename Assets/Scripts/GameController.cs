using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Generator generator;
	public HUDController hud;

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

	// Use this for initialization
	void Start () {
		if (collisionGenerationTime == 0)
			collisionGenerationTime = 1.0f;
		timeSpent = 0.0f;
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
		moveSpeedY += moveSpeedIncr;
		timeSpent += Time.deltaTime;
		score += moveSpeedY / 1000.0f + timeSpent / 100.0f;
		if (score >= scoreTarget) {
			LevelUp();
		}
		hud.UpdateHUD (this);
	}
}
