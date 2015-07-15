using UnityEngine;
using System.Collections;

public interface IGenerationScript {

	bool IsFinished();

	void OnScriptSelected();

	void PostGenerationAction(GameObject obj);
}
