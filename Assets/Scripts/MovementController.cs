using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public float yVelocity;
	public float xVelocity;

    public Vector3 accelRefPosition;
    public float sensibilityInc = 1.0f;

    public float[] sensibilityIncValues;

	//private Rigidbody body;
    private CharacterController charctrl;

	// Use this for initialization
	void Start () {
        charctrl = this.GetComponent<CharacterController>();
		//body = this.GetComponent<Rigidbody> ();
	}

	public void ApplyMovement(Vector3 vec)
	{
		if (GameController.gameState != GameState.PLAYING)
			return;
		if (GameController.isPaused == true)
			return;
        charctrl.Move(new Vector3(vec.x * (sensibilityInc * 0.5f), 0,
            vec.z * (sensibilityInc * 0.5f)));
		//body.AddForce(new Vector3(vec.x * 50.0f, 0, vec.z * 50.0f));
	}

    void KeyboardUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
            return;

        if (Input.GetAxis("Horizontal") > 0)
        {
            charctrl.Move(new Vector3(xVelocity * sensibilityInc, 0, 0));
            //body.Set(new Vector3(xVelocity, 0, 0));
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            charctrl.Move(new Vector3(-xVelocity * sensibilityInc, 0, 0));
            //body.AddRelativeForce(new Vector3(-xVelocity, 0, 0));
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            charctrl.Move(new Vector3(0, 0, yVelocity * sensibilityInc));
            //body.AddRelativeForce(new Vector3(0, 0, yVelocity));
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            charctrl.Move(new Vector3(0, 0, -yVelocity * sensibilityInc));
            //body.AddRelativeForce(new Vector3(0, 0, -yVelocity));
        }
        charctrl.Move(new Vector3(0, 0, 0.00001f));
    }
	
    void TouchInputUpdate()
    {
        Vector2 mv = new Vector2(0, 0);
        float scW = Screen.width / 2;
        float scH = Screen.height / 2;

        if (Application.platform != RuntimePlatform.Android)
            return;

        foreach (Touch touch in Input.touches) 
        {
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
               mv += new Vector2((scW - touch.position.x) / -scW,
                   (scH - touch.position.y) / -scH);
            }
        }
        mv.Normalize();
        /*if (mv.x != 0.0f && mv.y != 0.0f)
        {*/
            charctrl.Move(new Vector3(mv.x * xVelocity * sensibilityInc, 0, mv.y * yVelocity * sensibilityInc));
        /*}*/
    }

    void GyroUpdate()
    {
        if (SystemInfo.supportsAccelerometer)
        {
            Gyroscope gyro = Input.gyro;
            Vector3 acc = Input.acceleration - accelRefPosition;

            acc.x *= 4 * sensibilityInc;
            acc.y *= 4 * sensibilityInc;

            charctrl.Move(new Vector3(acc.x * xVelocity, 0, acc.y * yVelocity));
            GameObject.Find("TestSQ").transform.position = acc;
        }
    }

	// Update is called once per frame
	void Update () {
        if (GameController.gameState != GameState.PLAYING)
			return;
		if (GameController.isPaused == true)
			return;

        sensibilityInc = sensibilityIncValues[(int)GameController.sensibility];

        KeyboardUpdate();
        if (GameController.activeInput == InputType.TOUCH)
        {
            TouchInputUpdate();
        }
        if (GameController.activeInput == InputType.GYRO)
        {
            GyroUpdate();
        }
        if (GameController.activeInput == InputType.DPAD)
        {
            TouchInputUpdate();
        }
    }

    public void Reposition()
    {
        if (SystemInfo.supportsAccelerometer)
        {
            Gyroscope gyro = Input.gyro;
            Vector3 acc = Input.acceleration;

            accelRefPosition = acc;
        }
    }

    void PutLine(string text, int offset)
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, offset, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        GUI.Label(rect, text, style);
        rect.position = new Vector2(1, offset + 1);
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        GUI.Label(rect, text, style);
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        int lh = h * 2 / 100;
        int offset = 30;

        string text;
        /*GameObject obj = GameObject.Find("TestSQ");
        Vector3 gmr = obj.transform.position;

        text = string.Format("{0},{1},{2} acc", gmr.x, gmr.y, gmr.z);
        PutLine(text, offset);
        offset += lh;*/

        text = string.Format("{0},{1},{2} ref acc", accelRefPosition.x, accelRefPosition.y, accelRefPosition.z);
        PutLine(text, offset);
        offset += lh;

        text = string.Format("{0} active sensibility multiplier", sensibilityInc);
        PutLine(text, offset);
        offset += lh;

        text = string.Format("{0},{1} active movement speed", xVelocity, yVelocity);
        PutLine(text, offset);
        offset += lh;
    }
}
