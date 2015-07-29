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
        charctrl.Move(new Vector3(vec.x * 50.0f, 0, vec.z * 50.0f));
		//body.AddForce(new Vector3(vec.x * 50.0f, 0, vec.z * 50.0f));
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.gameState != GameState.PLAYING)
			return;
		if (GameController.isPaused == true)
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
}
