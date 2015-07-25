using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

	#region PROPS
	// Main GC
	public GameController controller;

	// Generation containers
	public WorldGenerationPossibility[] worlds;

	// Generic colliders that can be generated (unimplemented)
	//public ObjectGenerationPossibility[] collidersPrefabs;

	// Generic generation scripts alterators (unimplemented)
	public List<ObjectGenerationPossibility> generationScripts;
	
	// Generation parameters
	// +alterable by generation scripts
	// +alterable by difficulty state
	public Vector3 scale;
	public float rotation;

	// Active generation world
	public World activeWorld;

	// Entity spawn location
	public GameObject spawnLocation;

	// Entity despawn location
	public GameObject destroyLocation;

	// Entities global parent
	public GameObject spawnParent;
	#endregion
	#region PRIVATE_PROPS
	// Lowest block generated
	public GameObject lowestBlock;

	// Generation active script
	private IGenerationScript activeGenerationScript;

	// Fatal exception - loop breaker
	private bool fatal = false;
	//private float colliderGenerationTime;
	//private ArrayList generatedObjects;
	#endregion

	// Use this for initialization
	void Start () {
		if (worlds.Length == 0) {
			throw new UnityException("No worlds detected - cannot generate");
		}
		Clear ();
		SetNewWorld ();
		InitialSpawn ();
	}

	public void InitialSpawn() {
		/*
		float lastHeight = 0;
		GameObject prevObj = null;

		activeGenerationScript = generationScripts [0] as IGenerationScript;

		for (float i = spawnLocation.transform.position.y; i < destroyLocation.transform.position.y; i += lastHeight) {
			GameObject obj = GenerateWall();
		
			if (prevObj) {
				obj.transform.position = (new Vector3 (0, i - GetObjectLowerBound(obj), 0));
				lastHeight = GetObjectUpperBound(obj) - GetObjectLowerBound(obj);
			} else {
				obj.transform.position = (new Vector3 (0, i, 0));
				lastHeight = GetObjectUpperBound(obj);
			}
			prevObj = obj;

		}*/
	}

	// Update is called once per frame
	void Update () {
		ApplyMovement ();
		Generate ();
		ClearEntities ();
		/*
		//Transform[] ts = spawnParent.GetComponentsInChildren<Transform>();
		GameObject lastGo = null;
		bool notDeleted = false;

		if (GameController.isPaused)
			return;

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
				yDiff = lastGo.transform.position.y +
					GetObjectLowerBound(lastGo) -
					GetObjectUpperBound(obj);
			}
			obj.transform.position = new Vector3(spawnLocation.transform.position.x, yDiff, spawnLocation.transform.position.z);
		}
		colliderGenerationTime -= Time.deltaTime;
		if (colliderGenerationTime < 0) {
			colliderGenerationTime = controller.collisionGenerationTime;
			GenerateCollider();
		}
		UpdateGenerationScript ();*/
	}

	void UpdateGenerationScript()
	{
		/*
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
		activeGenerationScript.OnScriptSelected ();*/
	}

	public GameObject GenerateCollider() {
		return (null);
		/*
		GameObject prefab = null;
		while (prefab == null) {
			GameObjectChance goc = collidersPrefabs[Random.Range(0, collidersPrefabs.Length)];
			
			if (Random.Range(0, 100) < goc.chance) {
				prefab = goc.obj;
			}
		}

		GameObject obj = Instantiate<GameObject> (prefab);
		Rigidbody rbd = obj.GetComponent<Rigidbody> ();
		generatedObjects.Add (obj);
		obj.transform.position = spawnLocation.transform.position;
		obj.transform.SetParent (spawnParent.transform);
		if (rbd) {
			rbd.AddForce(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f)); 
			rbd.angularVelocity.Set (Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
		}
		return (obj);*/
	}

	#region METHODS
	// Reset all default data
	public void Clear() {
		bool cleared = false;
		scale = new Vector3 (3, 3, 3);
		rotation = 0.0f;
		activeGenerationScript = null;
		lowestBlock = null;

		foreach (Transform child in spawnParent.transform) {
			Destroy (child.gameObject);
		} 
	}

	private void ApplyMovement() {
		GameObject obj = null;

		foreach (Transform child in spawnParent.transform) {
			obj = child.gameObject;
			obj.transform.position += new Vector3 (0, controller.moveSpeedY, 0);
		} 
	}

	private void ClearEntities() {
		int childs = spawnParent.transform.childCount;
		Transform obj = null;

		foreach (Transform child in spawnParent.transform) {
			if (child.position.y > destroyLocation.transform.position.y) {
				Destroy (child.gameObject);
			}
		} 
	}

	public float GetSpawnLocationY() {
		return (spawnLocation.transform.position.y);
	}

	public GameObject GetLowestBlock() {
		return (lowestBlock);
	}

	public void Generate() {
		if (activeWorld == null)
			return ;
		if (activeWorld.CanGenerateBlock ()) {
			GameObject obj = activeWorld.GenerateBlock();
			ApplyGenerationParameters(obj);
			UpdateWorldsGenerationChance();
			lowestBlock = obj;
			fatal = true;
		}
	}

	// Apply generation specific settings to the last generated block
	public void ApplyGenerationParameters(GameObject obj) {
		if (!lowestBlock) {
			lowestBlock = spawnLocation;
		}
		obj.transform.localScale = scale;
		obj.transform.position = lowestBlock.transform.position + 
			new Vector3(0, BlockUtils.GetLowerBoundValue(lowestBlock), 0) - 
			new Vector3(0, BlockUtils.GetUpperBoundValue(obj), 0);
		obj.transform.SetParent (spawnParent.transform);
		obj.transform.Rotate (new Vector3 (0, rotation, 0));
	}

	public World SetNewWorld() {
		bool found = false;
		int maxIt = 50;

		while (maxIt > 0 && found == false) {
			int rnd = Random.Range(0, 100);
			WorldGenerationPossibility pos = worlds[Random.Range(0, worlds.Length)];

			--maxIt;
			if (pos.CanGenerate(rnd, controller.currentLevel)) {
				pos.ResetGenerateChance();
				activeWorld = pos.blueprint;
				found = true;
			}
		}
		if (maxIt == 0) {
			throw new UnityException("World generation failed - Cant find world matching parameters");
			fatal = true;
		}
		Debug.Log ("World changed to " + activeWorld.name);
		activeWorld.OnSet (this);
		return (activeWorld);
	}
	
	public void UpdateWorldsGenerationChance() {
		foreach (WorldGenerationPossibility obj in worlds) {
			obj.DecreaseGenerateChanceAfterActive();
		}
	}
	#endregion
}
