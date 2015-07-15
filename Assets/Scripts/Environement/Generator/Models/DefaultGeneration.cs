using UnityEngine;
using System.Collections;

public class DefaultGeneration : IGenerationScript {

	private int nbIterations;
	private Generator generator;

	public DefaultGeneration(Generator gen) 
	{
		generator = gen;
	}

	public bool IsFinished()
	{
		return (nbIterations > 3);
	}
	
	public void OnScriptSelected()
	{
		Debug.Log ("DefaultGeneration");
		nbIterations = 0;
	}
	
	public void PostGenerationAction(GameObject obj)
	{
		nbIterations++;
	}
}
