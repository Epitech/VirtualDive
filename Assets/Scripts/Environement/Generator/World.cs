using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class World {

	#region PROPS
	// Name identifier
	public string name = "Unknown World";

	// Connection element on top for other worlds
	public GameObject topConnector;

	// Connection element on bottom for other worlds
	public GameObject bottomConnector;

	// Elements that can be generated for this world
	public ObjectGenerationPossibility[] blockPrefabs;

	// Obstacles that can be generated for this world
	public ObjectGenerationPossibility[] obstaclePrefabs;
	
	// Generated objects count
	public int generatedCount;

	// Next Obstacle Generation ticks
	public int nextObstacleGenerateTicks;

	#endregion
	#region PRIVATE_PROPS
	// Local generator
	private Generator generator;
	#endregion
	#region METHODS
	// Occurs when the generator sets the world
	public void OnSet(Generator gen) {
		generator = gen;
		generatedCount = 0;
		Debug.Log ("World has been set to identifier 0");
	}

	// Checks if the block can be generated
	public bool CanGenerateBlock() {
		if (!generator.GetLowestBlock() || !BlockUtils.IsValid(generator.GetLowestBlock())) {
			return (true);
		}
		float offset = generator.GetLowestBlock().transform.position.y + BlockUtils.GetLowerBoundValue (generator.GetLowestBlock());

		if (offset > generator.GetSpawnLocationY())
			return (true);
		return (false);
	}

	// Generate a block
	public GameObject GenerateBlock() {
		GameObject prefab = null;
		int maxIt = 50;

		while (maxIt != 0 && prefab == null) {
			ObjectGenerationPossibility goc = blockPrefabs[Random.Range(0, blockPrefabs.Length)];
			--maxIt;
			if (goc.CanGenerate(Random.Range (0, 100), generator.controller.currentLevel)) {
				goc.ResetGenerateChance();
				prefab = goc.blueprint;
			}
		}
		if (maxIt == 0) {
			throw new UnityException("Generation iterations maximum of 50 reached - Invalid generation settings detected - Preventing inf. loop");
		}
		
		GameObject obj = GameObject.Instantiate<GameObject>(prefab);
		UpdateBlocksGenerationChance ();
		return (obj);
	}

	public void UpdateBlocksGenerationChance() {
		foreach (ObjectGenerationPossibility obj in blockPrefabs) {
			obj.DecreaseGenerateChanceAfterActive();
		}
	}

	// Generate an obstacle
	public GameObject GenerateObstacle() {

		return (null);
	}

	#endregion
}
