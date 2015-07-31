using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public float yVelocity;
	public float xVelocity;

	public delegate void typeRes();
	public typeRes toto;

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
        charctrl.Move(new Vector3(vec.x, 0, vec.z));
		//body.AddForce(new Vector3(vec.x * 50.0f, 0, vec.z * 50.0f));
	}

    void KeyboardUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
            return;

        if (Input.GetAxis("Horizontal") > 0)
        {
            charctrl.Move(new Vector3(xVelocity, 0, 0));
            //body.Set(new Vector3(xVelocity, 0, 0));
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            charctrl.Move(new Vector3(-xVelocity, 0, 0));
            //body.AddRelativeForce(new Vector3(-xVelocity, 0, 0));
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            charctrl.Move(new Vector3(0, 0, yVelocity));
            //body.AddRelativeForce(new Vector3(0, 0, yVelocity));
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            charctrl.Move(new Vector3(0, 0, -yVelocity));
            //body.AddRelativeForce(new Vector3(0, 0, -yVelocity));
        }
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
            if (touch.phase == TouchPhase.Began) {
               mv += new Vector2((scW - touch.position.x) / -scW,
                   (scH - touch.position.y) / -scH);
            }
        }
        if (mv.x != 0.0f && mv.y != 0.0f)
        {
            charctrl.Move(new Vector3(mv.x * xVelocity, 0, mv.y * yVelocity));
        }
    }

	// Update is called once per frame
	void Update () {
		if (GameController.gameState != GameState.PLAYING)
			return;
		if (GameController.isPaused == true)
			return;
        KeyboardUpdate();
        TouchInputUpdate();
	}
}
