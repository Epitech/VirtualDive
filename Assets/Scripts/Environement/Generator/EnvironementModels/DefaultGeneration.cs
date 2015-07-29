using UnityEngine;
using System.Collections;

public class DefaultGeneration : GenerationScript {

	public new bool IsFinished()
	{
		return (nbIterations > 3);
	}

    public new void OnScriptSelected()
	{
		Debug.Log ("DefaultGeneration");
		nbIterations = 0;
	}

    public new void PostGenerationAction(GameObject obj)
	{
		nbIterations++;
	}
}
