using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

	// Generation containers
	public GameObject[] wallsPrefabs;
	public GameObject[] collidersPrefabs;
	public List<Tuple<int, IGenerationScript>> generationScripts;
	
	// Generation parameters
	// +alterable by generation scripts
	// +alterable by difficulty state
	public float scaleX;
	public float scaleY;
	public float rotation;

	// Generation locations
	public GameObject spawnLocation;
	public GameObject destroyLocation;
	public GameObject spawnParent;

	// Internal informations
	public IList generatedObjects;
	public GameController controller;

	private IGenerationScript activeGenerationScript;
	private float colliderGenerationTime;
	//private ArrayList generatedObjects;

	// Use this for initialization
	void Start () {
		generatedObjects = new ArrayList();
		generationScripts = new List<Tuple<int, IGenerationScript>>();

		generationScripts.Add (new Tuple<int, IGenerationScript>(50, new DefaultGeneration (this)));
		generationScripts.Add (new Tuple<int, IGenerationScript>(25, new RotationGeneration (this)));
        generationScripts.Add (new Tuple<int, IGenerationScript>(25, new ScaleGeneration (this)));
		activeGenerationScript = null;

		colliderGenerationTime = controller.collisionGenerationTime;
		InitialSpawn ();
	}

	void InitialSpawn() {
		float lastHeight = 0;

		activeGenerationScript = generationScripts [0] as IGenerationScript;

		for (float i = spawnLocation.transform.position.y; i < destroyLocation.transform.position.y; i += lastHeight) {

			GameObject obj = GenerateWall();
			obj.transform.position += (new Vector3 (0, -i, 0));
			lastHeight = controller.offsetY;

		}
	}

	// Update is called once per frame
	void Update () {
		//Transform[] ts = spawnParent.GetComponentsInChildren<Transform>();
		GameObject lastGo = null;
		bool notDeleted = false;

		while (!notDeleted) {
			notDeleted = true;
			lastGo = null;
			foreach (GameObject obj in generatedObjects) {
				//GameObject obj = objTrs.gameObject;
				Transform objTrs = obj.transform;

				obj.transform.position = new Vector3 (objTrs.position.x, objTrs.position.y + controller.moveSpeedY, objTrs.position.z);
				if (objTrs.position.y > destroyLocation.transform.position.y) {
					generatedObjects.Remove (obj);
					Destroy (obj);
					notDeleted = false;
					break;
				}
				if (obj.tag == "Environement") {
					if (lastGo == null)
						lastGo = obj;
					if (lastGo != null && lastGo.transform.position.y > obj.transform.position.y)
						lastGo = obj;
				}
			}
		}
		if (lastGo == null || IsEntityOutOfSpawnRange(lastGo)) {
			GameObject obj = GenerateWall();
			float yDiff = spawnLocation.transform.position.y;

			if (lastGo) {
				yDiff = lastGo.transform.position.y -
					lastGo.transform.Find("LowerBound").transform.localPosition.y -
					obj.transform.Find("UpperBound").transform.localPosition.y +
					obj.transform.Find("LowerBound").transform.localPosition.y;
			}
			obj.transform.position = new Vector3(spawnLocation.transform.position.x, yDiff, spawnLocation.transform.position.z);
		}
		colliderGenerationTime -= Time.deltaTime;
		if (colliderGenerationTime < 0) {
			colliderGenerationTime = controller.collisionGenerationTime;
			GenerateCollider();
		}
		UpdateGenerationScript ();
	}

	void UpdateGenerationScript()
	{
		if (activeGenerationScript != null) {
			if (!activeGenerationScript.IsFinished()) {
				return ;
			}
		}
		activeGenerationScript = null;
		while (activeGenerationScript == null) {
			Tuple<int, IGenerationScript> tpl = generationScripts [Random.Range (0, generationScripts.Count)] as Tuple<int, IGenerationScript>;

			if (Random.Range(0, 100) < tpl.First) {
				activeGenerationScript = tpl.Second;
			}
		}
		activeGenerationScript.OnScriptSelected ();
	}

	public bool IsEntityOutOfSpawnRange(GameObject obj) {
		if (!obj.transform.Find ("UpperBound")) {
			return (false);
		}
		float size = obj.transform.Find("UpperBound").position.y - obj.transform.Find("LowerBound").position.y;

		if (obj.transform.position.y > spawnLocation.transform.position.y + size)
			return (true);
		return (false);
	}

	public GameObject GenerateWall() {
		GameObject prefab = wallsPrefabs[Random.Range(0, wallsPrefabs.Length)];
		GameObject obj = Instantiate<GameObject> (prefab);
		
		generatedObjects.Add (obj);
		obj.transform.position = spawnLocation.transform.position;
		obj.transform.SetParent (spawnParent.transform);
		obj.transform.Rotate (new Vector3 (0, rotation, 0));
		obj.transform.localScale = new Vector3 (scaleX, 1.0f, scaleY);
		if (activeGenerationScript != null) {
			activeGenerationScript.PostGenerationAction(obj);
		}
		return obj;
	}

	public GameObject GenerateCollider() {
		GameObject prefab = collidersPrefabs[Random.Range(0, collidersPrefabs.Length)];
		GameObject obj = Instantiate<GameObject> (prefab);

		generatedObjects.Add (obj);
		obj.transform.position = spawnLocation.transform.position;
		obj.transform.SetParent (spawnParent.transform);
		return (obj);
	}

	public void Generate() {

	}
}
