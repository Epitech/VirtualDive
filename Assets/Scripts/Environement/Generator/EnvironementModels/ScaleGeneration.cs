using UnityEngine;
using System.Collections;

public class ScaleGeneration : IGenerationScript {

	private Generator generator;

	private float targetX;
	private float targetY;

	public float incrValueX = 0.0f;
	public float incrValueY = 0.0f;
	public float minScale = 0.5f;
	public float maxScale = 2.0f;

	public int minIncrTicks = 5;
	public int maxIncrTicks = 15;

	public int ticksCountX = 0;
	public int ticksCountY = 0;

	public ScaleGeneration(Generator gen) 
	{
		generator = gen;
	}
	
	public bool IsFinished()
	{
		return (generator.scale.x == targetX && generator.scale.y == targetY);
	}
	
	public void OnScriptSelected()
	{
		Debug.Log ("ScaleGeneration");

		targetX = Random.Range (minScale, maxScale);
		targetY = Random.Range (minScale, maxScale);
		ticksCountX = Random.Range (minIncrTicks, maxIncrTicks);
		ticksCountY = Random.Range (minIncrTicks, maxIncrTicks);
		incrValueX = (targetX - generator.scale.x) / ticksCountX;
		incrValueY = (targetY - generator.scale.y) / ticksCountY;
	}
	
	public void PostGenerationAction(GameObject obj)
	{
		ticksCountX--;
		ticksCountY--;
		if (ticksCountX > 0) {
			generator.scale += new Vector3(incrValueX, 0, 0);
		} else {
			generator.scale = new Vector3(targetX, generator.scale.y, generator.scale.z);;
		}
		if (ticksCountY > 0) {
			generator.scale += new Vector3(0, 0, incrValueY);
		} else {
			generator.scale = new Vector3(generator.scale.y, targetY, generator.scale.z);;
		}
	}
}
