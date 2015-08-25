using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

	public GameController gc;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Collision");

        if (col.tag == "Collider")
        {
            gc.OnPlayerCollision();
        }
    }

    //TODO: fix colliders and add rigid bodies to prevent compound colliders
    void OnControllerColliderHit(ControllerColliderHit hitcol)
    {
        Collider col = hitcol.collider;
        Debug.Log("Collision");

        if (col.tag == "Collider")
        {
			gc.OnPlayerCollision();
		}
	}

	void OnTriggerExit() 
    {

	}
}
