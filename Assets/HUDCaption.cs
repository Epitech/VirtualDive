using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDCaption : MonoBehaviour {

	public Image background;
	public Text titleText;
	public Text descText;

	private float targetDir = 0.0f;

	// Use this for initialization
	void Start () {
		background.color = new Color (0, 0, 0, 0);
		titleText.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		descText.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		background.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp (background.color.a, targetDir * 0.7f, 0.1f));
		titleText.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp (titleText.color.a, targetDir * 1.0f, 0.1f));
		descText.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp (descText.color.a, targetDir * 1.0f, 0.1f));
		/*	background.CrossFadeAlpha (targetDir * 0.7f, 1.0f, true);
		titleText.CrossFadeAlpha (targetDir, 1.0f, true);
		descText.CrossFadeAlpha (targetDir, 1.0f, true);
		*/
	}

	IEnumerator ActionHide() {
		yield return new WaitForSeconds (2.0f);
		targetDir = 0.0f;
	}

	public void Hide() {
		StartCoroutine ("ActionHide");
	}

	public void Show(string title, string text) {
		targetDir = 1.0f;
		background.color = new Color (0, 0, 0, 0);
		//background.CrossFadeAlpha (0.7f, 1.0f, true);
		titleText.text = title;
		titleText.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		//titleText.CrossFadeAlpha (1.0f, 1.0f, true);
		descText.text = text;
		descText.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		//descText.CrossFadeAlpha (1.0f, 1.0f, true);
		StartCoroutine ("ActionHide");
	}
}
