using UnityEngine;
using System.Collections;

public class EnvironementController : MonoBehaviour {

    public GameObject[] randomFallingPrefabs;

    public int nextRandomFallingAt = 5;

	private Generator generator;
    

	// Use this for initialization
	void Start () 
    {
        generator = GetComponent<Generator>();	
	}

    GameObject GenerateFallingEntity()
    {
        // Generate a random pos. inside a XYZ sphere at spawn location
        Vector3 pos = Random.insideUnitSphere * 15;
        pos.y += generator.destroyLocation.transform.position.y;

        GameObject prefab = randomFallingPrefabs[Random.Range(0, randomFallingPrefabs.Length - 1)];
        GameObject obj = Instantiate(prefab);
        obj.transform.position = pos;
        obj.transform.parent = generator.spawnParent.transform;
        return (obj);
    }

	// Update is called once per frame
	void Update () {
        if (generator.generatedCount >= nextRandomFallingAt)
        {
            GenerateFallingEntity();
            nextRandomFallingAt += Random.Range(0, 8);
        }
	}
}
