﻿using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public float yVelocity;
	public float xVelocity;

	public delegate void typeRes();
	public typeRes toto;

	private Rigidbody body;

	// Use this for initialization
	void Start () {
		body = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Horizontal") > 0)
		{
			body.AddForce(new Vector3(xVelocity, 0, 0));
		}
		if (Input.GetAxis("Horizontal") < 0)
		{
			body.AddForce(new Vector3(-xVelocity, 0, 0));
		}
		if (Input.GetAxis("Vertical") > 0)
		{
			body.AddForce(new Vector3(0, 0, yVelocity));
		}
		if (Input.GetAxis("Vertical") < 0)
		{
			body.AddForce(new Vector3(0, 0, -yVelocity));
		}
	}
}
