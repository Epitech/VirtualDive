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

	public HUDController hud;
    public Camera uiCam;
    public bool isOculusUI;
    public float activeFocusTimer = 0.0f;
    public float focusTime = 1.5f;

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
        if (UnityEngine.VR.VRDevice.isPresent)
        {
            if (!isOculusUI)
            {
                // Rotate dynamically fixed UI
                panelHUD.transform.localRotation = new Quaternion(UnityEngine.VR.InputTracking.GetLocalRotation(0).x * -0.8f, 
                    UnityEngine.VR.InputTracking.GetLocalRotation(0).y * -1.2f, 0.0f, 1.0f);
            }

            if (isOculusUI && !uiLocked)
            { 
                // Raytrace pointer location
                Ray ray = uiCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit[] hits = Physics.RaycastAll(ray);
                bool focused = false;

                foreach (RaycastHit hit in hits)
                {
                    GameObject obj = hit.collider.gameObject;
                    Button bt = obj.transform.parent.gameObject.GetComponent<Button>();
                    if (bt)
                    {
                        events.SetSelectedGameObject(bt.gameObject);
                        focused = true;
                    }
                }
                activeFocusTimer -= Time.deltaTime;
                if (focused == false)
                {
                    activeFocusTimer = focusTime;
                    events.SetSelectedGameObject(null);
                }
                if (activeFocusTimer <= 0.0f)
                {
                    events.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
                    // Simulate lock
                    activeFocusTimer = 150.0f;
                }
            }
        }
	}
}
