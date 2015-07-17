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
		return (generator.scaleX == targetX && generator.scaleY == targetY);
	}
	
	public void OnScriptSelected()
	{
		Debug.Log ("ScaleGeneration");

		targetX = Random.Range (minScale, maxScale);
		targetY = Random.Range (minScale, maxScale);
		ticksCountX = Random.Range (minIncrTicks, maxIncrTicks);
		ticksCountY = Random.Range (minIncrTicks, maxIncrTicks);
		incrValueX = (targetX - generator.scaleX) / ticksCountX;
		incrValueY = (targetY - generator.scaleY) / ticksCountY;
	}
	
	public void PostGenerationAction(GameObject obj)
	{
		ticksCountX--;
		ticksCountY--;
		if (ticksCountX > 0) {
			generator.scaleX += incrValueX;
		} else {
			generator.scaleX = targetX;
		}
		if (ticksCountY > 0) {
			generator.scaleY += incrValueY;
		} else {
			generator.scaleY = targetY;
		}
	}
}
