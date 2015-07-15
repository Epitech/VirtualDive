using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {

		if (col.tag == "Collider") {
		}
		Debug.Log ("Collision");
	}

	void OnTriggerExit() {

	}
}
