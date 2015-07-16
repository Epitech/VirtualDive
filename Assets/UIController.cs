using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	
	public GameObject panelHUD;
	public GameObject panelPause;
	public GameObject panelGameOver;
	public GameObject panelMainMenu;

	public void SetPauseVisible(bool state) {
		panelPause.SetActive (state);
	}
	
	public void HideAll() {
		panelHUD.SetActive (false);
		panelPause.SetActive (false);
		panelGameOver.SetActive (false);
		panelMainMenu.SetActive (false);
	}

	public void ShowGameStatePanel(GameState state) {
		switch (state) {
		case GameState.MAIN_MENU:
			HideAll ();
			panelMainMenu.SetActive(true);
			break;
		case GameState.GAMEOVER:
			HideAll ();
			panelGameOver.SetActive(true);
			break;
		case GameState.PLAYING:
			HideAll ();
			panelHUD.SetActive(true);
			break;
		default:
			break;
		}
	}

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
