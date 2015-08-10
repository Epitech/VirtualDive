using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum FadeState {
	NONE,
	FADEIN,
	FADEOUT
};

public class UIController : MonoBehaviour {
	
	public GameObject panelHUD;
	public GameObject panelPause;
	public GameObject panelGameOver;
	public GameObject panelMainMenu;
	public GameObject panelFade;
    public GameObject panelOptions;
    public GameObject panelScores;
    public GameObject oculusPanelFade;

	public HUDController hud;

    private string inputTargetBtn = "LeftSide/Buttons/InputBtn/Values";
    private string soundTargetBtn = "LeftSide/Buttons/SoundBtn/Values";
    private string musicTargetBtn = "LeftSide/Buttons/MusicBtn/Values";
    private string sensibTargetBtn = "LeftSide/Buttons/SensibilityBtn/Values";

	// When a fading is in progress, lock ui states
	public bool uiLocked = false;
	public FadeState state = FadeState.NONE;

    public EventSystem events;

	public void SetPauseVisible(bool state) {
		panelPause.SetActive (state);
		events.SetSelectedGameObject(panelPause.transform.FindChild("Resume").gameObject);
	}
	
	public void HideAll() {
		panelHUD.SetActive (false);
		panelPause.SetActive (false);
		panelGameOver.SetActive (false);
		panelMainMenu.SetActive (false);
        panelOptions.SetActive(false);
        panelScores.SetActive(false);
	}

	public void ShowGameStatePanel(GameState state) {
		switch (state) {
		case GameState.MAIN_MENU:
			HideAll ();
			panelMainMenu.SetActive(true);
            events.SetSelectedGameObject(
                panelMainMenu.transform.Find("LeftSide/Buttons/PlayBtn").gameObject);
            break;
        case GameState.OPTIONS:
            HideAll();
            panelOptions.SetActive(true);
            UpdateAllUI();
            events.SetSelectedGameObject(
                panelOptions.transform.Find("LeftSide/Buttons/InputBtn").gameObject);
            break;
        case GameState.SCORE:
            HideAll();
            panelScores.SetActive(true);
            UpdateAllUI();
            events.SetSelectedGameObject(
                panelScores.transform.Find("LeftSide/Buttons/TypeBtn").gameObject);
            break;
        case GameState.GAMEOVER:
			HideAll ();
			panelGameOver.SetActive(true);
			FadeGameOverBg();
			events.SetSelectedGameObject(panelGameOver.transform.FindChild("RetryBtn").gameObject);
			break;
		case GameState.PLAYING:
			HideAll ();
			panelHUD.SetActive(true);
			break;
		default:
			break;
		}
	}

	IEnumerator Fade(float originValue, float targetValue, float time, GameObject panel) {
		float dt = (targetValue - originValue) / time;
		float i = originValue;

		panel.GetComponent<Image>().color = new Color(0, 0, 0, originValue);
		panel.SetActive (true);
		while (true) {
			if ((dt > 0.0f && i > targetValue) || (dt < 0.0f && i < targetValue)) {
				break;
			}
		    panelFade.GetComponent<Image>().color = new Color(0, 0, 0, i);
			i += dt;
			yield return null;
		}
		panel.GetComponent<Image>().color = new Color(0, 0, 0, targetValue);
		if (targetValue == 0.0f) {
			panelFade.SetActive (false);
		}
		uiLocked = false;
		yield return null;
	}

	public void FadeGameOverBg() {
		//StartCoroutine(Fade (0.0f, 0.5f, 12.0f, panelGameOver));
		panelGameOver.GetComponent<Image>().color = new Color(0, 0, 0, 0.0f);
		panelGameOver.GetComponent<Image> ().CrossFadeColor (new Color(0, 0, 0, 0.5f), 1.0f, true, true);
	}

	public void FadeIn() {
		uiLocked = true;
		state = FadeState.FADEIN;
	}

	public void FadeOut() {
		uiLocked = true;
		state = FadeState.FADEOUT;
	}

	// Use this for initialization
	void Start () {
		//events = transform.FindChild ("EventSystem").GetComponent<EventSystem>();
        RepositionUIByResolution();
	}
	
    void RepositionUIByResolution()
    {
        float scrW = Screen.width;
        float scrH = Screen.height;
        float scrRatio = scrW / scrH;
        float uiW = GameObject.Find("UIs/UI/Canvas").GetComponent<RectTransform>().rect.width;
        float uiH = GameObject.Find("UIs/UI/Canvas").GetComponent<RectTransform>().rect.height;
        float uiRatio = uiW / uiH;
        Vector3 pos = GameObject.Find("UIs/UI/Canvas").transform.localPosition;

        Debug.Log("ScrRatio=" + scrRatio + " - UIRatio=" + uiRatio);
        if (scrRatio < uiRatio)
        {
            pos.z = (uiRatio - scrRatio) * 80.0f;
        }
        else
        {
            pos.z = 0;
        }
        GameObject.Find("UIs/UI/Canvas").transform.localPosition = pos;

    }

	// Update is called once per frame
	void Update () {
		switch (state) {
		case FadeState.FADEIN:
			StartCoroutine(Fade (0.0f, 1.0f, 12.0f, panelFade));
			break;
		case FadeState.FADEOUT:
			StartCoroutine(Fade (1.0f, 0.0f, 12.0f, panelFade));
			break;
		default:
			break;
		}
		state = FadeState.NONE;
        
	}

    public void UpdateAllUI()
    {
        UpdateOptionsUI(panelOptions.transform.Find(soundTargetBtn), GameController.soundState ? 1 : 0);
        UpdateOptionsUI(panelOptions.transform.Find(musicTargetBtn), GameController.musicState ? 1 : 0);
        UpdateOptionsUI(panelOptions.transform.Find(inputTargetBtn), (int)GameController.activeInput);
        UpdateOptionsUI(panelOptions.transform.Find(sensibTargetBtn), (int)GameController.sensibility);  
    }

    public void ToggleSound()
    {
        GameController.soundState = !GameController.soundState;
        UpdateOptionsUI(panelOptions.transform.Find(soundTargetBtn), GameController.soundState ? 1 : 0);
    }

    public void ToggleMusic()
    {
        GameController.musicState = !GameController.musicState;
        UpdateOptionsUI(panelOptions.transform.Find(musicTargetBtn), GameController.musicState ? 1 : 0);
    }

    public void ToggleInput()
    {
        GameController.activeInput++;
        if (GameController.activeInput >= InputType.MAX)
            GameController.activeInput = 0;
        UpdateOptionsUI(panelOptions.transform.Find(inputTargetBtn), (int)GameController.activeInput);
    }

    public void ToggleSensibility()
    {
        GameController.sensibility++;
        if (GameController.sensibility >= Sensibility.MAX)
            GameController.sensibility = 0;
        UpdateOptionsUI(panelOptions.transform.Find(sensibTargetBtn), (int)GameController.sensibility);
    }

    void UpdateOptionsUI(Transform values, int val)
    {
        foreach (Transform tr in values)
        {
            if (int.Parse(tr.name) == val)
            {
                tr.gameObject.SetActive(true);
            }
            else
            {
                tr.gameObject.SetActive(false);
            }
        }
    }
}

