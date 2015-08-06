using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
	
	// UI HUD
	public GameObject speedValue;
	public GameObject levelValue;
	public GameObject levelBar;
    public GameObject damageBar;
    public GameObject damageValue;
    public GameObject warningBoxLeft;

	public GameObject gameOverSpeedValue;
	public GameObject gameOverTimeValue;
	public GameObject gameOverLevelValue;
	public GameObject gameOverLevelBar;
	public GameObject gameOverRecord;

    public Color colorNoDamage;
    public Color colorDamage;
    public Color colorDanger;
    public Color colorOff;

	public HUDCaption hudCaption;

    public float damageVal = 0.0f;

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
		string dmgText;
		string speedText;

        damageVal = Mathf.Lerp(damageVal, gc.damage, 0.3f);
		
		speedText = string.Format("{0:000}.{1:00}", Mathf.Floor(gc.moveSpeedY * 15.0f), (Mathf.Floor(gc.moveSpeedY * 1500.0f)) % 100);
		speedValue.GetComponent<Text>().text = speedText;

        dmgText = string.Format("{0}", Mathf.Floor(damageVal));
        damageValue.GetComponent<Text>().text = dmgText;
		
		// Update the bar
        levelBar.GetComponent<Image>().fillAmount = (gc.score / gc.scoreTarget) / 2.0f;
        damageBar.GetComponent<Image>().fillAmount = (damageVal / gc.maxDamage) / 2.0f;

        // Update the value positions
        float levelVal = levelBar.GetComponent<Image>().fillAmount * 2;
        Vector3 pos;
        pos = new Vector3(
            -400.0f * (levelVal * levelVal) + 400.0f * levelVal + 80.0f,
            270.0f * levelVal - 135.0f,
            0.0f
            );
        speedValue.transform.localPosition = pos;

        float damageValRadius = damageBar.GetComponent<Image>().fillAmount * 2;
        pos = new Vector3(
            420.0f * (damageValRadius * damageValRadius) - 420.0f * damageValRadius - 115.0f,
            270.0f * damageValRadius - 135.0f,
            0.0f
            );
        damageValue.transform.localPosition = pos;
        damageBar.transform.parent.GetComponent<Image>().color = colorOff;
        if (damageValRadius > 0)
        {
            if (damageValRadius > 0.68)
            {
                damageValue.GetComponent<Text>().color = colorDanger;
                damageValue.transform.Find("Label Metric").GetComponent<Text>().color = damageValue.GetComponent<Text>().color;
                damageBar.transform.parent.GetComponent<Image>().color = colorDanger;
                warningBoxLeft.SetActive(true);
            }
            else
            {
                damageValue.GetComponent<Text>().color = colorDamage;
                damageValue.transform.Find("Label Metric").GetComponent<Text>().color = damageValue.GetComponent<Text>().color;
                warningBoxLeft.SetActive(false);
            }
        }
        else
        {
            damageValue.GetComponent<Text>().color = colorNoDamage;
            damageValue.transform.Find("Label Metric").GetComponent<Text>().color = damageValue.GetComponent<Text>().color;
            warningBoxLeft.SetActive(false);
        }

        /*RectTransform barTr = levelBar.GetComponent<RectTransform> ();
        float ratio = gc.score / gc.scoreTarget;
		
        barTr.anchoredPosition = new Vector2 (120.0f * ratio, barTr.anchoredPosition.y);
        barTr.sizeDelta = new Vector2 (240.0f * ratio, barTr.sizeDelta.y);*/
	}
}
