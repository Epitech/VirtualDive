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
	//private IGenerationScript activeGenerationScript;

	// Fatal exception - loop breaker
	//private bool fatal = false;
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
        Vector3 origPos = destroyLocation.transform.position;
        Vector3 activePos = origPos;
        GameObject obj;

        lowestBlock = null;
        while (activePos.y > spawnLocation.transform.position.y)
        {
            float yDiff = 0.0f;

            obj = activeWorld.GenerateBlock();
            ApplyGenerationParameters(obj);
            UpdateWorldsGenerationChance();
            if (lowestBlock)
                yDiff = BlockUtils.GetUpperBoundValue(obj);
            obj.transform.position = new Vector3(activePos.x, activePos.y - yDiff, activePos.z);
            
            if (!lowestBlock)
                activePos += new Vector3(0, BlockUtils.GetLowerBoundValue(obj), 0);
            else
                activePos -= new Vector3(0, BlockUtils.GetHeight(obj), 0);

            lowestBlock = obj;
        }
		//TODO
	}

	// Update is called once per frame
	void Update () {
		if (GameController.isPaused)
			return;

		ApplyMovement ();
		Generate ();
		ClearEntities ();
		activeWorld.GenerateObstacle ();
	}

	#region METHODS
	// Reset all default data
	public void Clear() {
		//bool cleared = false;
		scale = new Vector3 (3, 3, 3);
		rotation = 0.0f;
		//activeGenerationScript = null;
		lowestBlock = null;

		foreach (Transform child in spawnParent.transform) {
			Destroy (child.gameObject);
		} 
	}

	private void ApplyMovement() {
		GameObject obj = null;

		foreach (Transform child in spawnParent.transform) {
			obj = child.gameObject;
			obj.transform.position += new Vector3 (0, controller.ApplyTimeScale(controller.moveSpeedY), 0);
		} 
	}

	private void ClearEntities() {
		int childs = spawnParent.transform.childCount;
		//Transform obj = null;

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
			//fatal = true;
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
			if (pos.CanGenerate(rnd, controller.currentLevel, lowestBlock)) {
				pos.ResetGenerateChance();
				activeWorld = pos.blueprint;
				found = true;
			}
		}
		if (maxIt == 0) {
			throw new UnityException("World generation failed - Cant find world matching parameters");
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
