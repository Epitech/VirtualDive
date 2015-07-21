using UnityEngine;
using System.Collections;
using VR = UnityEngine.VR;

public class OVRCharacterController : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 target;

	private int trackCount = 0;

	public MovementController mvtCtrl;

	// Use this for initialization
	void Start () {

	}

	Vector3 TrackerPosToUnity(float x, float y, float z) {
		return (new Vector3(x * 260, 0, y * 700));	
	}

    // Update is called once per frame
	void Update () {
		if (!OVRManager.tracker.isPresent)
			return;

		Vector3 poseTracker = VR.InputTracking.GetLocalPosition (VR.VRNode.Head);
		Vector3 pose = TrackerPosToUnity(poseTracker.x, poseTracker.y, poseTracker.z) - startPos;
		Vector3 distance = pose - transform.position;
		target = pose;

		Debug.Log ("Target is at " + target + ", dist is " + distance);
		//transform.position = new Vector3 (pose.x * 150, 0, pose.y * 260);

		mvtCtrl.ApplyMovement (distance);
		//prevPos = pose;

		if (Input.GetKeyUp (KeyCode.R)) {
			OVRManager.display.RecenterPose();
			trackCount = 0;
			poseTracker = VR.InputTracking.GetLocalPosition (VR.VRNode.Head);
			pose = TrackerPosToUnity(poseTracker.x, poseTracker.y, poseTracker.z);
			transform.position = new Vector3 (0, 0, 0);
			startPos = pose;
		}
	}
}
