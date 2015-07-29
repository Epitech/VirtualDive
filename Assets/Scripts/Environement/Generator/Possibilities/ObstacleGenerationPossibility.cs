using UnityEngine;

[System.Serializable]
public class ObstacleGenerationPossibility : GenerationPossibility<Obstacle> 
{
    public GameObject[] spawnOnBlockPrefabs;

    public bool BlockInAllow(GameObject block)
    {
        foreach (GameObject obj in spawnOnBlockPrefabs)
        {
            if (obj.name == block.name) {
                return (true);
            }
        }
        return (false);
    }

    new public bool CanGenerate(int randChance, int activeLevel, GameObject blockReference)
    {
        return (iterationsBeforeNextGenerationActive == 0 && randChance < chance &&
                activeLevel >= minLevel && activeLevel <= maxLevel && BlockInAllow(blockReference));
    }
}