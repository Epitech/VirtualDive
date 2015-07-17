using UnityEngine;
using System.Collections;

public class RotationGeneration : IGenerationScript {

	private Generator generator;
	private float targetRot = 0.0f;

	public RotationGeneration(Generator gen) 
	{
		generator = gen;
	}

	public bool IsFinished()
	{
		return (generator.rotation >= targetRot - 1.0f);
	}
	
	public void OnScriptSelected()
	{
		Debug.Log ("RotationGeneration");
		//generator.rotation = 0;
		targetRot = generator.rotation + 90.0f;
	}
	
	public void PostGenerationAction(GameObject obj)
	{
		generator.rotation += 5;
	}
}
