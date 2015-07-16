using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
	
	// UI HUD
	public GameObject speedValue;
	public GameObject timeValue;
	public GameObject levelValue;
	public GameObject levelBar;

	public void OnLevelUp(int newLevel)
	{		
		levelValue.GetComponent<Text>().text = "LEVEL " + newLevel;
	}


	// Use this for initialization
	void Start () {
	
	}

	void Update() {
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
