using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
	
	// UI HUD
	public GameObject speedValue;
	public GameObject timeValue;
	public GameObject levelValue;
	public GameObject levelBar;

	public GameObject gameOverSpeedValue;
	public GameObject gameOverTimeValue;
	public GameObject gameOverLevelValue;
	public GameObject gameOverLevelBar;
	public GameObject gameOverRecord;

	public HUDCaption hudCaption;

	public void OnLevelUp(int newLevel)
	{		
		levelValue.GetComponent<Text>().text = "LEVEL " + newLevel;
	}


	// Use this for initialization
	void Start () {
	
	}

	void Update() {
	}

	public void OnShow() {
	}

	public void UpdateGameOverHUD(GameController gc) {
		string timeText;
		string speedText;
		
		timeText = string.Format("{0:00}:{1:00}", Mathf.Floor(gc.timeSpent), (Mathf.Floor(gc.timeSpent  * 100)) % 100);
		speedText = string.Format("{0:000}.{1:00}", Mathf.Floor(gc.moveSpeedY * 15.0f), (Mathf.Floor(gc.moveSpeedY * 1500.0f)) % 100);
		gameOverSpeedValue.GetComponent<Text>().text = speedText;
		gameOverTimeValue.GetComponent<Text>().text = timeText;

		// Update new record
		gameOverRecord.SetActive (false);

		// Update level
		gameOverLevelValue.GetComponent<Text>().text = "LEVEL" + gc.currentLevel;

		// Update the bar
		RectTransform barTr = gameOverLevelBar.GetComponent<RectTransform> ();
		float ratio = gc.score / gc.scoreTarget;
		
		barTr.anchoredPosition = new Vector2 (205.0f * ratio, barTr.anchoredPosition.y);
		barTr.sizeDelta = new Vector2 (410.0f * ratio, barTr.sizeDelta.y);
	}

	// Update is called once per frame
	public void UpdateHUD (GameController gc) {
		string timeText;
		string speedText;
		
		timeText = string.Format("{0:00}:{1:00}", Mathf.Floor(gc.timeSpent), (Mathf.Floor(gc.timeSpent  * 100)) % 100);
		speedText = string.Format("{0:000}.{1:00}", Mathf.Floor(gc.moveSpeedY * 15.0f), (Mathf.Floor(gc.moveSpeedY * 1500.0f)) % 100);
		speedValue.GetComponent<Text>().text = speedText;
		timeValue.GetComponent<Text>().text = timeText;
		
		// Update the bar
		RectTransform barTr = levelBar.GetComponent<RectTransform> ();
		float ratio = gc.score / gc.scoreTarget;
		
		barTr.anchoredPosition = new Vector2 (120.0f * ratio, barTr.anchoredPosition.y);
		barTr.sizeDelta = new Vector2 (240.0f * ratio, barTr.sizeDelta.y);
	}
}
