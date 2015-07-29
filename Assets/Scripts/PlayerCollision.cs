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

    void OnControllerColliderHit(ControllerColliderHit hitcol)
    {
        Collider col = hitcol.collider;
        Debug.Log("Collision");
        if (col.tag == "Collider")
        {
			gc.OnPlayerCollision();
		}
	}

	void OnTriggerExit() {

	}
}
