using UnityEngine;
using System.Collections;

public class RotationGeneration : GenerationScript {

	private float targetRot = 0.0f;

	public new bool IsFinished()
	{
		return (generator.rotation >= targetRot - 1.0f);
	}

    public new void OnScriptSelected()
	{
		Debug.Log ("RotationGeneration");
		targetRot = generator.rotation + 90.0f;
	}
	
	public new void PostGenerationAction(GameObject obj)
	{
		generator.rotation += 5;
	}
}
