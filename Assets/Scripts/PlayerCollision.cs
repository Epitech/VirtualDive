using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

	public GameController gc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {

		if (col.tag == "Collider") {
			Debug.Log ("Collision");
			gc.OnPlayerCollision();
		}
	}

	void OnTriggerExit() {

	}
}
