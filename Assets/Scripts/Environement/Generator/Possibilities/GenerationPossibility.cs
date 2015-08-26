using UnityEngine;
using System.Collections;

[System.Serializable]
public class GenerationPossibility<T> {

	// The blueprint to be spawned
	public T blueprint;

	// The chance from 0-100 for the object to be selected
	public int chance = 50;

	// Ammount of any instances before the object can be instantiated again
	public int iterationsBeforeNextGeneration = 0;

	// Minimum level to be considered for generation
	public int minLevel = 0;

	// Maximum level to be considered for generation
	public int maxLevel = 100;

	// Internals
	// Active count from 0 to inf before generating
	protected int iterationsBeforeNextGenerationActive = 0;
	
	public void ResetGenerateChance() {
		iterationsBeforeNextGenerationActive = iterationsBeforeNextGeneration;
	}
	
	public void DecreaseGenerateChanceAfterActive() {
		if (iterationsBeforeNextGenerationActive != 0)
			--iterationsBeforeNextGenerationActive;
	}

    public bool LevelValid(int activeLevel)
    {
        return ((activeLevel >= minLevel && (activeLevel <= maxLevel || maxLevel == 0)));
    }

	public bool CanGenerate(int randChance, int activeLevel, GameObject blockReference) {
		return (iterationsBeforeNextGenerationActive == 0 && randChance < chance &&
                LevelValid(activeLevel));
	}
}
